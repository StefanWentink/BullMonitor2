using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class ZacksTickerSyncMessageHandler
        : BaseMessageHandler<ZacksTickerSyncMessage>
    {
        protected ISingleProvider<ZacksRequest, ZacksResponse> ZacksProvider { get; }
        protected ICreator<ZacksEntity> ZacksCreator { get; }
        protected ICompanyUpdater CompanyUpdater { get; }

        public ZacksTickerSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISingleProvider<ZacksRequest, ZacksResponse> zacksProvider,
            ICompanyUpdater companyUpdater,
            ICreator<ZacksEntity> zacksCreator,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ZacksTickerSyncMessageHandler> logger)
            : base(
                  issueMessageSender,
                  dateTimeOffsetNow,
                  logger)
        {
            ZacksCreator = zacksCreator;
            ZacksProvider = zacksProvider;
            CompanyUpdater = companyUpdater;
        }

        public async override Task<MessageHandlingResponse> Handle(
            ZacksTickerSyncMessage value,
            CancellationToken cancellationToken)
        {
            var request = new ZacksRequest(
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

                var known = response?.Found;

                if (response is not null && known == true)
                {
                    var record = new ZacksEntity(response.Ticker)
                    {
                        ReferenceDate = value.ReferenceDate,
                        Value = new ZacksValue(
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

                                response.AverageBrokerRecommendation)
                    };

                    var created = await ZacksCreator
                       .Create(record, cancellationToken)
                       .ConfigureAwait(false);

                    var entityMessage = $"{nameof(ZacksValue)} for {nameof(ZacksEntity.Ticker)} '{record.Ticker}' and {nameof(ZacksEntity.ReferenceDate)} '{record.ReferenceDate}'";

                    var createMessage = string.IsNullOrWhiteSpace(created.Ticker)
                        ? $"{entityMessage}' already existed."
                        : $"Created {entityMessage}'.";

                    Logger
                        .LogInformation($"{nameof(ZacksTickerSyncMessageHandler)}: {createMessage}");
                }
                else
                {
                    Logger
                        .LogInformation($"{nameof(ZacksTickerSyncMessageHandler)}: {value.Code}' was not known.");
                }

                if (known is null)
                {
                    Logger
                        .LogInformation($"{nameof(ZacksTickerSyncMessageHandler)}: {value.Code}' returned no unambiguous answer of it being found");
                }
                else if (value.UpdateKnown)
                {
                    Logger
                        .LogInformation($"{nameof(ZacksTickerSyncMessageHandler)}: updating {value.Code}' to known {known.Value}.");

                    await CompanyUpdater
                        .SetKnownByZacks(value.Code, known.Value, cancellationToken)
                        .ConfigureAwait(false);
                }

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