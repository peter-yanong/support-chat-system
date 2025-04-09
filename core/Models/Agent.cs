using MoneyBase.Core.Enums;

namespace MoneyBase.Core.Models
{
    public class Agent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AgentLevel Seniority { get; set; }
        public int CurrentChats { get; set; }

        public int MaxChats => (int)(10 * Seniority.Efficiency());

        public bool IsAvailable => CurrentChats < MaxChats;
    }
}
