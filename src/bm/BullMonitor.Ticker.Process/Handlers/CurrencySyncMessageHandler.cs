using BullMonitor.Abstractions.Commands;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.Ticker.Process.Handlers
{
    public class CurrencySyncMessageHandler
        : SyncMessageHandler<CurrencySyncMessage, CurrencyEntity, string>
    {
        protected override Dictionary<string, CurrencyEntity> Collection => new()
        {
            { "USD", new CurrencyEntity("USD", "Dollar") },
            { "EUR", new CurrencyEntity("EUR", "Euro") }
        };

        public CurrencySyncMessageHandler(
            ISqlProvider<CurrencyEntity> provider,
            ICollectionAndSingleCreator<CurrencyEntity> creator,
            IMessageSender<IssueMessage> issueMessageSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<CurrencySyncMessageHandler> logger)
            : base(
                 provider,
                 creator,
                 issueMessageSender,
                 dateTimeOffsetNow,
                 logger)
        { }
    }
}