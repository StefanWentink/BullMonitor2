namespace NieuweStroom.Infrastructure.RabbitMq.Interfaces
{
    public interface IMessageReceiver<T>
        : IDisposable
    {
        Task StartReceiving(CancellationToken cancellationToken);
    }
}