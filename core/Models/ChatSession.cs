namespace MoneyBase.Core.Models
{
    public class ChatSession
    {
        public Guid SessionId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
