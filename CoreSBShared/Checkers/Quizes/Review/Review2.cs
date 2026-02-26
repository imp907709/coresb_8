using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace CoreSBShared.Universal.Checkers.Quizes.Review;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Age { get; set; }
}

public class UserCreateRequest
{
    public string Username { get; set; }
    public int Age { get; set; }
}

public class UserResponse : UserCreateRequest
{
    public int Id { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=test.db");
    }
}


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPut("create-user")]
    public async Task<ActionResult<UserResponse>> CreateUser(UserCreateRequest userRequest)
    {
        using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var dbUser = new User { Username = userRequest.Username, Age = userRequest.Age };
        try
        {
            await _context.Users.AddAsync(dbUser);
            await _context.SaveChangesAsync();
            tran.Complete();
        }
        catch
        {
            return Conflict(new { message = "user already exists" });
        }

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync("http://192.168.54.38/api/notify", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "user_id", dbUser.Id.ToString() }
            }));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Internal service failed.");
            }
        }

        var factory = new ConnectionFactory() { HostName = "192.168.55.34" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "user_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var body = System.Text.Encoding.UTF8.GetBytes(dbUser.Id.ToString());
        await channel.BasicPublishAsync(exchange:"", routingKey:"user_queue", mandatory:false, new ReadOnlyMemory<byte>(body));
        return new UserResponse { Id = dbUser.Id, Username = dbUser.Username, Age = dbUser.Age };
    }

    [HttpGet("get-user")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        var dbUser = await _context.Users.FindAsync(userId);
        if (dbUser == null)
        {
            return NotFound();
        }

        return new UserResponse { Id = dbUser.Id, Username = dbUser.Username, Age = dbUser.Age };
    }
}


