using BullMonitor.Abstractions.Commands;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Time.Extensions;
using SWE.Time.Utilities;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class ZacksSyncMessageHandler
        : TickerValueSyncMessageHandler<ZacksSyncMessage, ZacksTickerSyncMessage>
    {
        public ZacksSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISender<ZacksTickerSyncMessage> messageSender,
            ICompanyProvider companyProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TickerValueSyncMessageHandler<ZacksSyncMessage, ZacksTickerSyncMessage>> logger)
            : base(
                  issueMessageSender,
                  messageSender,
                  companyProvider,
                  dateTimeOffsetNow,
                  logger)
        { }

        protected override ZacksTickerSyncMessage ToSendTask(
            Guid id,
            string code,
            bool? known)
        {
            return new ZacksTickerSyncMessage(
                id,
                code,
                DateTimeOffset.Now.SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo),
                known == null);
        }

        protected override bool? GetKnown(CompanyListResponse value)
        {
            return value.KnownByZacks;
        }

        protected override Task<IEnumerable<CompanyListResponse>> GetCompanies(
            CancellationToken cancellationToken)
        {
            return CompanyProvider
                .GetKnownByZacks(cancellationToken);
        }
    }
}