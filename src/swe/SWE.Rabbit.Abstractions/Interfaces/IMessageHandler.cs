using SWE.Rabbit.Abstractions.Enums;

namespace SWE.Rabbit.Abstractions.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task<MessageHandlingResponse> Handle(
            T value,
            CancellationToken cancellationToken);
    }
}