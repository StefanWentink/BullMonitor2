using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Rabbit.Abstractions.Handlers
{
    /// <summary>
    /// Legacy implementation for <see cref="IHandler<T>"/> implementations.
    /// If the inner Handler returns <see cref="CancellationToken"/> not cancelled, the message is <see cref="MessageHandlingResponse.Successful"/>.
    /// If the inner Handler returns <see cref="CancellationToken"/> cancelled, the message is <see cref="MessageHandlingResponse.Unsuccessful"/>.
    /// If the inner Handler throws an exception the message is <see cref="MessageHandlingResponse.Rejected"/>.
    /// </summary>
    public class MessageHandler<T>
        : IMessageHandler<T>
    {
        protected IHandler<T> Handler { get; }
        protected ILogger<MessageHandler<T>> Logger { get; }

        public MessageHandler(
            IHandler<T> handler,
            ILogger<MessageHandler<T>> logger)
        {
            Handler = handler;
            Logger = logger;
        }

        public async Task<MessageHandlingResponse> Handle(
            T value,
            CancellationToken cancellationToken)
        {
            try
            {
                await Handler
                    .Handle(value, cancellationToken)
                    .ConfigureAwait(false);

                return cancellationToken.IsCancellationRequested
                    ? MessageHandlingResponse.Unsuccessful
                    : MessageHandlingResponse.Successful;
            }
            catch (Exception exception)
            {
                var exceptionMessage = $"{nameof(Handler)} threw an exception resulting in '{MessageHandlingResponse.Rejected}': {exception.Message}";
                Logger.LogError(exception, exceptionMessage);
                return MessageHandlingResponse.Rejected;
            }
        }
    }
}