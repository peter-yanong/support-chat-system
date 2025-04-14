using RabbitMQ.Client;

namespace MoneyBase.Core.Services.Impl
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private IConnection _connection;
        private readonly string _hostname;

        private Task Initiliaze { get; }

        public RabbitMQService(string hostname)
        {
            _hostname = hostname;
            Initiliaze = InitConnection();
        }

        private async Task InitConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname
            };
            _connection = await factory.CreateConnectionAsync();
        }

        public IConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    Task.WaitAll(InitConnection());
                    return _connection;
                }
                return _connection;
            }
        }

        public Dictionary<string, object> QueueArgs =>
            new Dictionary<string, object>
                {
                    { "x-max-length", 24 },
                    { "x-overflow", "reject-publish" }
                };

        public void Dispose()
        {
            if (_connection != null) _connection.Dispose();
        }

        public async Task<(IChannel, string)> QueueDeclareAsync(string queueName)
        {
            try
            {
                var channel = await _connection.CreateChannelAsync();
                await channel.QueueDeclareAsync(
                    queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: queueName == "session-support" ? QueueArgs : null);
                return (channel, queueName);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
