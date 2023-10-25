namespace BullMonitor.Abstractions.Commands
{
    public  record TickerValueSyncMessage(
        Guid TickerId,
        string Code,
        DateTimeOffset ReferenceDate,
        bool UpdateKnown)
    { }
}