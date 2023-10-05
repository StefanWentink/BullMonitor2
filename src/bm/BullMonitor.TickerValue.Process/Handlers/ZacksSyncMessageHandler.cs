using BullMonitor.Abstractions.Commands;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class ZacksSyncMessageHandler
        : TickerValueSyncMessageHandler<ZacksSyncMessage, ZacksTickerSyncMessage>
    {
        public ZacksSyncMessageHandler(
            IMessageSender<IssueModel> issueModelSender,
            ISender<ZacksTickerSyncMessage> messageSender,
            ISqlProvider<TickerEntity> tickerProvider,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TickerValueSyncMessageHandler<ZacksSyncMessage, ZacksTickerSyncMessage>> logger)
            : base(
                  issueModelSender,
                  messageSender,
                  tickerProvider,
                  dateTimeOffsetNow,
                  logger)
        { }

        protected override ZacksTickerSyncMessage ToSendTask(
            Guid id,
            string code)
        {
            return new ZacksTickerSyncMessage(id, code, DateTimeOffset.Now);
        }
    }
}