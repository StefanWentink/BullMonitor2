namespace SWE.Rabbit.Abstractions.Interfaces
{
    public interface IMessageReceiver<T>
        : IDisposable
    {
        Task StartReceiving(CancellationToken cancellationToken);
    }
}