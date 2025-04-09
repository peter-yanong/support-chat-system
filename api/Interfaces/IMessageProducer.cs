namespace Moneybase.API.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message);
    }
}
