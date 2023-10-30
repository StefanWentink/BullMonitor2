namespace BullMonitor.Abstractions.Requests
{
    public record ZacksRequest(
            DateTimeOffset From,
            DateTimeOffset Until,
            string Ticker)
        { }
}