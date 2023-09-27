using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Rabbit.Abstractions.Messages;

namespace SWE.Process.Handlers
{
    public abstract class BaseMessageHandler<TMessage>
        : IMessageHandler<TMessage>
    {

#if DEBUG
        private static readonly int _lockMilliSecondsTimeOut = 120_000;
#else
        private static readonly int _lockMilliSecondsTimeOut = 30_000;
#endif

#if DEBUG
        private static readonly int _lockSize = 1;
#else
        private static readonly int _lockSize = 12;
#endif

        protected int LockMilliSecondsTimeOut => _lockMilliSecondsTimeOut;
        protected int LockSize => _lockSize;

        protected CancellationToken MainProcessCancellationToken { get; private set; }
        protected IMessageSender<IssueModel> IssueModelSender { get; }
        protected IDateTimeOffsetNow DateTimeOffsetNow { get; }
        protected ILogger<BaseMessageHandler<TMessage>> Logger { get; }

        protected BaseMessageHandler(
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<BaseMessageHandler<TMessage>> logger)
        {
            IssueModelSender = issueModelSender;
            DateTimeOffsetNow = dateTimeOffsetNow;
            Logger = logger;
        }

        protected async Task SendIssue(
            IssueModel message,
            CancellationToken cancellationToken)
        {
            await IssueModelSender
                .Send(new RoutingMessage<IssueModel>(message.LogLevel.ToString(), message), cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendIssue(
            LogLevel logLevel,
            string description,
            CancellationToken cancellationToken)
        {
            var message = new IssueModel(
                GetType().Name,
                logLevel,
                description,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendExceptionIssue(
            Exception exception,
            CancellationToken cancellationToken)
        {
            Logger.LogError(exception, exception.Message);

            var message = new IssueModel(
                GetType().Name,
                LogLevel.Error,
                exception.Message,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }

        public abstract Task<MessageHandlingResponse> Handle(
            TMessage value,
            CancellationToken cancellationToken);
    }
}