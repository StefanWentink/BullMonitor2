using BullMonitor.Data.Storage.Interfaces;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.Ticker.Process.Handlers
{
    public abstract class SyncMessageHandler<TMessage, T, TKey>
        : BaseMessageHandler<TMessage>
        where T : class, IIdCode
    {
        protected virtual int LockWaitTimeMilliSeconds => 20_000;
        protected ISqlProvider<T> Provider { get; }
        protected ICollectionAndSingleCreator<T> Creator { get; }

        private readonly SemaphoreSlim _lock = new(1);
        protected abstract Dictionary<string, T> Collection { get; }

        protected SyncMessageHandler(
            ISqlProvider<T> provider,
            ICollectionAndSingleCreator<T> creator,
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<SyncMessageHandler<TMessage, T, TKey>> logger)
            : base(
                  issueModelSender,
                  dateTimeOffsetNow,
                  logger)
        {
            Provider = provider;
            Creator = creator;
        }

        public override async Task<MessageHandlingResponse> Handle(
            TMessage value,
            CancellationToken cancellationToken)
        {
            try
            {
                await _lock
                    .WaitAsync(LockWaitTimeMilliSeconds, cancellationToken)
                    .ConfigureAwait(false);

                var conditionContainer = new SqlConditionContainer<T>(
                    x => Collection.Keys.Contains(x.Code));

                var values = await Provider
                    .Get(conditionContainer, cancellationToken)
                    .ConfigureAwait(false);

                var creates = Collection
                    .Where(x => !values.Select(v => v.Code).Contains(x.Key))
                    .Select(x => x.Value)
                    .ToList();

                if (creates.Any())
                {
                    var created = await Creator
                        .Create(creates, cancellationToken)
                        .ConfigureAwait(false);

                    Logger.LogInformation($"Created {created.Count()} {typeof(T).Name}.");
                    return MessageHandlingResponse.Successful;
                }

                Logger.LogInformation($"No {typeof(T).Name} to create.");
                return MessageHandlingResponse.Successful;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return MessageHandlingResponse.Unsuccessful;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}