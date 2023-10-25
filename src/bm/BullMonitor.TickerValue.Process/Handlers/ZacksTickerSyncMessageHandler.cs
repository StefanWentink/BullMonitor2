using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class ZacksTickerSyncMessageHandler
        : BaseMessageHandler<ZacksTickerSyncMessage>
    {
        protected ISingleProvider<ZacksRankRequest, ZacksRankResponse> ZacksProvider { get; }
        protected ICreator<ZacksRankEntity> ZacksRankCreator { get; }
        protected ICompanyUpdater CompanyUpdater { get; }

        public ZacksTickerSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISingleProvider<ZacksRankRequest, ZacksRankResponse> zacksProvider,
            ICompanyUpdater companyUpdater,
            ICreator<ZacksRankEntity> zacksRankCreator,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ZacksTickerSyncMessageHandler> logger)
            : base(
                  issueMessageSender,
                  dateTimeOffsetNow,
                  logger)
        {
            ZacksRankCreator = zacksRankCreator;
            ZacksProvider = zacksProvider;
            CompanyUpdater = companyUpdater;
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
                                response.VGM,

                                response.AveragePriceTarget,
                                response.LowestPriceTarget,
                                response.HighestPriceTarget,
                                response.PriceTargetPercentage,

                                response.StrongBuy,
                                response.Buy,
                                response.Hold,
                                response.Sell,
                                response.StrongSell,

                                response.AverageBrokerRecommendation
                            )
                    };

                    var created = await ZacksRankCreator
                       .Create(record, cancellationToken)
                       .ConfigureAwait(false);

                    if (value.UpdateKnown)
                    {
                        var known = true;
                        await CompanyUpdater
                            .SetKnownByZacks(value.Code, known, cancellationToken)
                            .ConfigureAwait(false);
                    }

                    var entityMessage = $"{nameof(ZacksRankValue)} for {nameof(ZacksRankEntity.Ticker)} '{record.Ticker}' and {nameof(ZacksRankEntity.ReferenceDate)} '{record.ReferenceDate}'";

                    var createMessage = string.IsNullOrWhiteSpace(created.Ticker)
                        ? $"{entityMessage}' already existed."
                        : $"Created {entityMessage}'.";

                    Logger
                        .LogInformation(createMessage);

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