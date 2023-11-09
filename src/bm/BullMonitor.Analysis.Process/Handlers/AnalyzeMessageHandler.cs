using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;
using BullMonitor.Abstractions.Commands;

namespace BullMonitor.Analysis.Process.Handlers
{
    public abstract class AnalyzeMessageHandler
        : BaseMessageHandler<AnalyzeMessage>
    {
        protected ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksEntity> ZacksProvider { get; }
        protected ISingleProvider<(string ticker, DateTimeOffset referenceDate), TipRanksEntity> TipranksProvider { get; }
        protected ICompanyProvider CompanyProvider { get; }

        public AnalyzeMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksEntity> zacksProvider,
            ISingleProvider<(string ticker, DateTimeOffset referenceDate), TipRanksEntity> tipranksProvider,
            ICompanyProvider companyProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<AnalyzeMessageHandler> logger)
            : base(
                  issueMessageSender,
                  dateTimeOffsetNow,
                  logger)
        {
            ZacksProvider = zacksProvider;
            TipranksProvider = tipranksProvider;
            CompanyProvider = companyProvider;
        }
        
        public async override Task<MessageHandlingResponse> Handle(
            AnalyzeMessage value,
            CancellationToken cancellationToken)
        {
            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var tickers = await CompanyProvider
                    .GetKnownByAll(cancellationToken)
                    .ConfigureAwait(false);

                // TODO Implement

                //var tickerPairs = tickers
                //    .DistinctBy(x => x.Code)
                //    .Select(x => (x.Id, x.Code, Known: GetKnown(x)))
                //    .ToList();

                //if (tickerPairs.Any())
                //{
                //    var sendTasks = tickerPairs
                //        .Select(tickerPair => ToSendTask(tickerPair.Id, tickerPair.Code, tickerPair.Known))
                //        .Select(sendTask => MessageSender.Send(sendTask, cancellationToken));

                //    await Task.WhenAll(sendTasks).ConfigureAwait(false);

                //    Logger.LogInformation($"Send {tickerPairs.Count()} {typeof(TOut).Name}.");
                //    return MessageHandlingResponse.Successful;
                //}

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
