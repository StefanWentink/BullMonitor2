using SWE.Rabbit.Abstractions.Messages;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Rabbit.Abstractions.Interfaces
{
    /// <summary>
    /// Interafces to be used as parent interface in DI
    /// Depend on individual ISender<>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageSender<T>
        : ISender<RoutingMessage<T>>
        , ISender<T>
    { }
}