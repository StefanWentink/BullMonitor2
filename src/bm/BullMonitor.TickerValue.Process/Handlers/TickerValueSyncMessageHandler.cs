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

namespace BullMonitor.TickerValue.Process.Handlers
{
    public abstract class TickerValueSyncMessageHandler<TIn, TOut>
        : BaseMessageHandler<TIn>
        where TOut : class
    {
        protected ISender<TOut> MessageSender { get; }
        protected ISqlProvider<TickerEntity> TickerProvider { get; }

        public TickerValueSyncMessageHandler(
            IMessageSender<IssueModel> issueModelSender,
            ISender<TOut> messageSender,
            ISqlProvider<TickerEntity> tickerProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TickerValueSyncMessageHandler<TIn, TOut>> logger)
            : base(
                  issueModelSender,
                  dateTimeOffsetNow,
                  logger)
        {
            MessageSender = messageSender;
            TickerProvider = tickerProvider;
        }
        
        public async override Task<MessageHandlingResponse> Handle(
            TIn value,
            CancellationToken cancellationToken)
        {
            ISqlConditionContainer<TickerEntity> condition = new SqlConditionContainer<TickerEntity>();

            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var tickers = await TickerProvider
                    .Get(condition, cancellationToken)
                    .ConfigureAwait(false);

                var tickerPairs = tickers
                    .DistinctBy(x => x.Code)
                    .Select(x => (x.Id, x.Code))
                    .ToList();

                if (tickerPairs.Count > 0)
                {
                    var sendTasks = tickerPairs
                        .Select(tickerPair => ToSendTask(tickerPair.Id, tickerPair.Code))
                        .Select(sendTask => MessageSender.Send(sendTask, cancellationToken));

                    await Task.WhenAll(sendTasks).ConfigureAwait(false);

                    Logger.LogInformation($"Send {tickerPairs.Count()} {typeof(TOut).Name}.");
                    return MessageHandlingResponse.Successful;
                }

                Logger.LogInformation($"No {typeof(TOut).Name} to send.");
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

        protected abstract TOut ToSendTask(Guid id, string code);
    }
}
