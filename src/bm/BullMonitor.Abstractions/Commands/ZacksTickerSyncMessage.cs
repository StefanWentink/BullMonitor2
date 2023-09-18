namespace BullMonitor.Abstractions.Commands
{
    public  record ZacksTickerSyncMessage(
        Guid TickerId,
        DateTimeOffset ReferenceDate)
    { }
}