using MoneyBase.Core.Models;

namespace MoneyBase.API.Interfaces
{
    public interface IChatService
    {
        Task<string> QueueSupportRequestAsync();
    }
}
