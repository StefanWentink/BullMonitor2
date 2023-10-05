namespace BullMonitor.Abstractions.Commands
{
    public  record ZacksTickerSyncMessage(
        Guid TickerId,
        string Code,
        DateTimeOffset ReferenceDate)
        : TickerValueSyncMessage(
            TickerId,
            Code,
            ReferenceDate)
    { }
}