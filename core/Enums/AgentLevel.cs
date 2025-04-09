namespace MoneyBase.Core.Enums
{
    public enum AgentLevel
    {
        Junior,
        MidLevel,
        Senior,
        TeamLead
    }

    public static class AgentLevelExtensions
    {
        public static double Efficiency(this AgentLevel level)
        {
            return level switch
            {
                AgentLevel.Junior => 0.4,
                AgentLevel.MidLevel => 0.6,
                AgentLevel.Senior => 0.8,
                AgentLevel.TeamLead => 0.5,
                _ => 0.4
            };
        }
    }
}
