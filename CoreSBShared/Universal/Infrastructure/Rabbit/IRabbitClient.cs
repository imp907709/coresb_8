using RabbitMQ.Client;

namespace CoreSBShared.Universal.Infrastructure.Rabbit;

public interface IRabbitClient
{
    IConnection GetConnection();
    ConnectionFactory CreateConnFactory();
    Task<IChannel> ChannelOpen();
    Task ChannelClose(IChannel channel);
    bool IsConnected();
}
