using MoneyBase.Core.Services.Impl;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MoneyBase.ChatCoordinator
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var rabbitMQService = new RabbitMQService("localhost");
            using var channel = await rabbitMQService.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "session-support", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;
            await channel.BasicConsumeAsync(queue: "session-support", autoAck: true, consumer: consumer);


            Console.ReadLine();
        }

        private static Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}
