namespace BullMonitor.Abstractions.Commands
{
    public  record ZacksTickerSyncMessage(
        Guid TickerId,
        string Code,
        DateTimeOffset ReferenceDate,
        bool UpdateKnown)
        : TickerValueSyncMessage(
            TickerId,
            Code,
            ReferenceDate,
            UpdateKnown)
    { }
}