namespace BullMonitor.Abstractions.Requests
{
    public record TipRanksRequest(
            DateTimeOffset From,
            DateTimeOffset Until,
            string Ticker)
        { }
}