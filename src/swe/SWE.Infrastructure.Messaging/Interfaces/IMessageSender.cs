using SWE.Infrastructure.Messaging.Messages;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace NieuweStroom.Infrastructure.RabbitMq.Interfaces
{
    /// <summary>
    /// Interfaces to be used as parent interface in DI
    /// Depend on individual ISender<>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageSender<T>
        : ISender<T>
        , ISender<RoutingMessage<T>>
    { }
}