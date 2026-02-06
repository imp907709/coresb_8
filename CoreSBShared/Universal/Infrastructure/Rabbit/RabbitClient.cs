using CoreSBShared.Registrations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CoreSBShared.Universal.Infrastructure.Rabbit;

public class RabbitClient : IRabbitClient
{
    private RabbitConfig  _config;
    private readonly IConnection _connection;
    
    public RabbitClient(IOptions<RabbitConfig> options)
    {
        _config = options.Value;
        var fac = CreateConnFactory();
        this._connection = fac.CreateConnectionAsync().GetAwaiter().GetResult();
    }

    public IConnection GetConnection() => this._connection;

    public ConnectionFactory CreateConnFactory()
    {
        ConnectionFactory fac = new ConnectionFactory()
        {
            UserName = _config.User,
            Password = _config.Password,
            HostName = _config.Host,
            Port = _config.Port
        };

        return fac;
    }
    
    public async Task<IChannel> ChannelOpen()
    {
        return await this._connection.CreateChannelAsync();
    }

    public async Task ChannelClose(IChannel channel)
    {
        await channel.CloseAsync();
        await channel.DisposeAsync();
    }

    public bool IsConnected()
    {
        return this._connection?.IsOpen ?? false;
    }
}
