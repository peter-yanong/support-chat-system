using MoneyBase.Core.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Moneybase.API.Interfaces.Impl
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IRabbitMQService _messageService;
        public RabbitMQProducer(IRabbitMQService rabbitMQConnectionService)
        {
            _messageService = rabbitMQConnectionService;
        }
        public async Task SendMessageAsync<T>(T message)
        {
            try
            {
                var (channel, quename) = await _messageService.QueueDeclareAsync("session-support");
                using (channel)
                {
                    var json = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(json);

                    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: quename, body: body);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
