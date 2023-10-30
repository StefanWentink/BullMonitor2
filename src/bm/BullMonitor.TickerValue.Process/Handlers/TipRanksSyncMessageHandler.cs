using BullMonitor.Abstractions.Commands;
using BullMonitor.Data.Storage.Models;
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
    public class TipRanksSyncMessageHandler
        : TickerValueSyncMessageHandler<TipRanksSyncMessage, TipRanksTickerSyncMessage>
    {
        public TipRanksSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISender<TipRanksTickerSyncMessage> messageSender,
            ICompanyProvider companyProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TickerValueSyncMessageHandler<TipRanksSyncMessage, TipRanksTickerSyncMessage>> logger)
            : base(
                  issueMessageSender,
                  messageSender,
                  companyProvider,
                  dateTimeOffsetNow,
                  logger)
        { }

        protected override TipRanksTickerSyncMessage ToSendTask(
            Guid id,
            string code,
            bool? known)
        {
            return new TipRanksTickerSyncMessage(
                id,
                code,
                DateTimeOffset.Now.SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo),
                known == null);
        }

        protected override bool? GetKnown(CompanyListResponse value)
        {
            return value.KnownByTipRanks;
        }

        protected override Task<IEnumerable<CompanyListResponse>> GetCompanies(
            CancellationToken cancellationToken)
        {
            return CompanyProvider
                .GetKnownByTipRanks(cancellationToken);
        }
    }
}