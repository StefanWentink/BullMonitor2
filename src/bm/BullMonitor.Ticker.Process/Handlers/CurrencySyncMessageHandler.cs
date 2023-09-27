using BullMonitor.Abstractions.Commands;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Models;
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
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<CurrencySyncMessageHandler> logger)
            : base(
                 provider,
                 creator,
                 issueModelSender,
                 dateTimeOffsetNow,
                 logger)
        { }
    }
}