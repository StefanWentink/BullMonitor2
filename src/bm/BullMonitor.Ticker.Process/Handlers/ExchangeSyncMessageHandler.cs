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
    public class ExchangeSyncMessageHandler
        : SyncMessageHandler<ExchangeSyncMessage, ExchangeEntity, string>
    {
        protected override Dictionary<string, ExchangeEntity> Collection => new()
        {
            { "AMEX", new ExchangeEntity("AMEX", "American Stock Exchange") },
            { "NASDAQ", new ExchangeEntity("NASDAQ", "Nasdaq") },
            { "NYSE", new ExchangeEntity("NYSE", "New York Stock Exchange") }
        };

        public ExchangeSyncMessageHandler(
            ISqlProvider<ExchangeEntity> provider,
            ICollectionAndSingleCreator<ExchangeEntity> creator,
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ExchangeSyncMessageHandler> logger)
            : base(
                 provider,
                 creator,
                 issueModelSender,
                 dateTimeOffsetNow,
                 logger)
        { }
    }
}