using RabbitMQ.Client;

namespace MoneyBase.Core.Services
{
    public interface IRabbitMQService
    {
        IConnection Connection { get; }

        Task<(IChannel, string)> QueueDeclareAsync(string queueName);
    }
}
