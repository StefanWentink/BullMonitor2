using SWE.Rabbit.Abstractions.Enumerations;

namespace SWE.Rabbit.Abstractions.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task<MessageHandlingResponse> Handle(
            T value,
            CancellationToken cancellationToken);
    }
}