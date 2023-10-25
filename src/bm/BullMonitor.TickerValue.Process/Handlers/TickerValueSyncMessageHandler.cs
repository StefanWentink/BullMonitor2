using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
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
        protected ICompanyProvider CompanyProvider { get; }

        public TickerValueSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISender<TOut> messageSender,
            ICompanyProvider companyProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TickerValueSyncMessageHandler<TIn, TOut>> logger)
            : base(
                  issueMessageSender,
                  dateTimeOffsetNow,
                  logger)
        {
            MessageSender = messageSender;
            CompanyProvider = companyProvider;
        }
        
        public async override Task<MessageHandlingResponse> Handle(
            TIn value,
            CancellationToken cancellationToken)
        {
            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var tickers = await GetCompanies(cancellationToken)
                    .ConfigureAwait(false);

                var tickerPairs = tickers
                    .DistinctBy(x => x.Code)
                    .Select(x => (x.Id, x.Code, Known: GetKnown(x)))
                    .ToList();

                if (tickerPairs.Any())
                {
                    var sendTasks = tickerPairs
                        .Select(tickerPair => ToSendTask(tickerPair.Id, tickerPair.Code, tickerPair.Known))
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

        protected abstract Task<IEnumerable<CompanyListResponse>> GetCompanies(CancellationToken cancellationToken);

        protected abstract TOut ToSendTask(Guid id, string code, bool? known);


        protected abstract bool? GetKnown(CompanyListResponse value);
    }
}
