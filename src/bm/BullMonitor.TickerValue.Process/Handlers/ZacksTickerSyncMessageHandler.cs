using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class ZacksTickerSyncMessageHandler
        : BaseMessageHandler<ZacksTickerSyncMessage>
    {
        //protected ISqlProvider<TickerEntity> TickerProvider { get; }
        protected ISingleProvider<ZacksRankRequest, ZacksRankResponse> ZacksProvider { get; }

        public ZacksTickerSyncMessageHandler(
            IMessageSender<IssueModel> issueModelSender,
            //ISqlProvider<TickerEntity> tickerProvider,
            ISingleProvider<ZacksRankRequest, ZacksRankResponse> zacksProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ZacksTickerSyncMessageHandler> logger)
            : base(
                  issueModelSender,
                  dateTimeOffsetNow,
                  logger)
        {
            //TickerProvider = tickerProvider;
            ZacksProvider = zacksProvider;
        }

        public async override Task<MessageHandlingResponse> Handle(
            ZacksTickerSyncMessage value,
            CancellationToken cancellationToken)
        {
            var request = new ZacksRankRequest(
                value.ReferenceDate,
                value.ReferenceDate,
                value.Code);

            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var response = await ZacksProvider
                    .GetSingleOrDefault(request, cancellationToken)
                    .ConfigureAwait(false);

                if (response is not null)
                {
                    // Upsert =>
                    //var sendTasks = tickerPairs
                    //    .Select(tickerPair => ToSendTask(tickerPair.Id, tickerPair.Code))
                    //    .Select(sendTask => MessageSender.Send(sendTask, cancellationToken));

                    //await Task.WhenAll(sendTasks).ConfigureAwait(false);

                    //Logger.LogInformation($"Send {tickerPairs.Count()} {typeof(TOut).Name}.");
                    return MessageHandlingResponse.Successful;
                }

                Logger.LogInformation($"No {typeof(ZacksRankResponse).Name} to send.");
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