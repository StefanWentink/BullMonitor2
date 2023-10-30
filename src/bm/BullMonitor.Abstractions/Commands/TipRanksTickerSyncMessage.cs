namespace BullMonitor.Abstractions.Commands
{
    public record TipRanksTickerSyncMessage(
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