using Moneybase.API.Interfaces;
using MoneyBase.Core.Models;

namespace MoneyBase.API.Interfaces.Impl
{
    public class ChatService : IChatService
    {
        private readonly IMessageProducer _messageProducer;
        public ChatService(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }
        public async Task<string> QueueSupportRequestAsync()
        {
            ChatSession session = new ChatSession { SessionId = Guid.NewGuid() };
            await _messageProducer.SendMessageAsync(session);
            return session.SessionId.ToString();
        }
    }
}
