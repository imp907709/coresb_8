using CoreSBBL.LogingTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreSBBL.Logging.Infrastructure.EF;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options) : base(options)
    {
        
    }

    public DbSet<LoginTest> LoginTest { get; set; }
    public DbSet<LogginAccountTest> LogginAccountTest { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
