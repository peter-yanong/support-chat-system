using MoneyBase.Core.Enums;

namespace MoneyBase.Core.Models
{
    public class Agent
    {
        public string Id { get; set; }
        public AgentLevel Seniority { get; set; }
        public int ActiveChats { get; set; }
        public bool IsInShift { get; set; }
        public string AgentQueueName
        {
            get
            {
                return $"{Seniority.ToString()}-queue";
            }
        }

        public int GetMaxCapacity()
        {
            return (int)(10 * Seniority.Efficiency());
        }

        public bool CanTakeChat()
        {
            return IsInShift && ActiveChats < GetMaxCapacity();
        }
    }
}
