using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
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
        protected ISingleProvider<ZacksRankRequest, ZacksRankResponse> ZacksProvider { get; }
        protected ICollectionAndSingleUpserter<ZacksRankEntity> ZacksUpserter { get; }

        public ZacksTickerSyncMessageHandler(
            IMessageSender<IssueModel> issueModelSender,
            ISingleProvider<ZacksRankRequest, ZacksRankResponse> zacksProvider,
            ICollectionAndSingleUpserter<ZacksRankEntity> zacksUpserter,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ZacksTickerSyncMessageHandler> logger)
            : base(
                  issueModelSender,
                  dateTimeOffsetNow,
                  logger)
        {
            ZacksUpserter = zacksUpserter;
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
                    var record = new ZacksRankEntity(response.Ticker)
                    {
                        ReferenceDate = value.ReferenceDate,
                            Value = new ZacksRankValue(
                                response.Rank,
                                response.Value,
                                response.Growth,
                                response.Momentum,
                                response.VGM
                            )
                    };

                    //Upsert =>
                    await ZacksUpserter
                       .Upsert(record, cancellationToken)
                       .ConfigureAwait(false);

                    Logger.LogInformation($"Upserter {nameof(ZacksRankValue)}.");
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