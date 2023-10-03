namespace BullMonitor.Abstractions.Commands
{
    public record TipRanksTickerSyncMessage(
        Guid TickerId,
        DateTimeOffset ConsensusReferenceDate,
        DateTimeOffset PriceReferenceDate,
        DateTimeOffset ScoreReferenceDate)
    { }
}