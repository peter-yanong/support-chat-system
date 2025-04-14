using MoneyBase.Core.Enums;
using MoneyBase.Core.Models;
using MoneyBase.Core.Services;
using MoneyBase.Core.Services.Impl;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MoneyBase.ChatCoordinator
{
    internal class Program
    {
        static List<Agent> _agents;
        static List<Agent> _overflowAgents;
        static RabbitMQService rabbitMQService;

        static async Task Main(string[] args)
        {

            #region Agent test

            _agents = new List<Agent>
            {
                new Agent { Id = "A1", Seniority = AgentLevel.MidLevel, ActiveChats = 4, IsInShift = true },
                new Agent { Id = "A2", Seniority = AgentLevel.Senior, ActiveChats = 8, IsInShift = true },
                new Agent { Id = "A3", Seniority = AgentLevel.Junior, ActiveChats = 3, IsInShift = false }
            };

            _overflowAgents = new List<Agent>
            {
                new Agent { Id = "O1", Seniority = AgentLevel.Junior, ActiveChats = 1, IsInShift = true }
            };

            #endregion




            rabbitMQService = new RabbitMQService("localhost");
            using var channel = await rabbitMQService.Connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "session-support", durable:  false, exclusive: false, autoDelete: false,
    arguments: rabbitMQService.QueueArgs);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;
            await channel.BasicConsumeAsync(queue: "session-support", autoAck: true, consumer: consumer);


            Console.ReadLine();
        }

        private static async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);


            // Agent queueing
            var assigner = new AgentAssignerService(_agents, _overflowAgents);
            var assignedAgent = assigner.AssignAgent();

            if (assignedAgent == null) throw new Exception("No available agent");

            Console.WriteLine($"Assigning agent to {assignedAgent.AgentQueueName}");
            try
            {
                var (channel, quename) = await rabbitMQService.QueueDeclareAsync(assignedAgent.AgentQueueName);
                using (channel)
                {
                    var json = JsonSerializer.Serialize(message);

                    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: quename, body: body);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return;
        }
    }
}
