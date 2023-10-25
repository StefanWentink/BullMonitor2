using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Issue.Abstractions.Enumerations;
using SWE.Issue.Abstractions.Messages;
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

        protected virtual int LockMilliSecondsTimeOut => _lockMilliSecondsTimeOut;
        protected virtual int LockSize => _lockSize;

        protected readonly SemaphoreSlim _lock = new(_lockSize);

        protected CancellationToken MainProcessCancellationToken { get; private set; }
        protected IMessageSender<IssueMessage> IssueMessageSender { get; }
        protected IDateTimeOffsetNow DateTimeOffsetNow { get; }
        protected ILogger<BaseMessageHandler<TMessage>> Logger { get; }

        protected BaseMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<BaseMessageHandler<TMessage>> logger)
        {
            IssueMessageSender = issueMessageSender;
            DateTimeOffsetNow = dateTimeOffsetNow;
            Logger = logger;
        }

        protected async Task SendIssue(
            IssueMessage message,
            CancellationToken cancellationToken)
        {
            await IssueMessageSender
                .Send(new RoutingMessage<IssueMessage>(message.LogLevel.ToString(), message), cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendIssue(
            LogLevel logLevel,
            string description,
            IssueClassification issueClassification,
            CancellationToken cancellationToken)
        {
            var message = new IssueMessage(
                GetType().Name,
                logLevel,
                description,
                issueClassification,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendExceptionIssue(
            Exception exception,
            CancellationToken cancellationToken)
        {
            Logger.LogError(exception, exception.Message);

            var message = new IssueMessage(
                GetType().Name,
                LogLevel.Error,
                exception.Message,
                IssueClassification.Exception,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }

        public abstract Task<MessageHandlingResponse> Handle(
            TMessage value,
            CancellationToken cancellationToken);
    }
}