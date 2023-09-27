using SWE.Infrastructure.Messaging.Enumerations;

namespace SWE.Infrastructure.Messaging.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task<MessageHandlingResponse> Handle(
            T value,
            CancellationToken cancellationToken);
    }
}