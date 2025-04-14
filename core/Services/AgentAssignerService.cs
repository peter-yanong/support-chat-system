using MoneyBase.Core.Models;

namespace MoneyBase.Core.Services
{
    public class AgentAssignerService
    {
        private readonly List<Agent> _agents;
        private readonly List<Agent> _overflowAgents;

        public AgentAssignerService(List<Agent> agents, List<Agent> overflowAgents)
        {
            _agents = agents;
            _overflowAgents = overflowAgents;
        }

        public Agent? AssignAgent()
        {
            // First try: assign to agent in current shift with available capacity
            var assigned = _agents
                .Where(a => a.CanTakeChat())
                .OrderBy(a => a.ActiveChats)
                .FirstOrDefault();

            if (assigned != null)
            {
                assigned.ActiveChats++;
                return assigned;
            }

            // If no available agent and it's during office hours, try overflow
            if (IsOfficeHours())
            {
                var overflow = _overflowAgents
                    .Where(a => a.ActiveChats < a.GetMaxCapacity())
                    .OrderBy(a => a.ActiveChats)
                    .FirstOrDefault();

                if (overflow != null)
                {
                    overflow.ActiveChats++;
                    return overflow;
                }
            }

            return null; // No agent available
        }
        private bool IsOfficeHours()
        {
            var now = DateTime.UtcNow.TimeOfDay;
            return now >= TimeSpan.FromHours(8) && now <= TimeSpan.FromHours(17);
        }
    }
}
