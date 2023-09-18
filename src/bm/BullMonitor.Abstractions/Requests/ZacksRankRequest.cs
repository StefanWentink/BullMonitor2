using System.Text.Json.Serialization;

namespace BullMonitor.Abstractions.Requests
{
    public record ZacksRankRequest(
            DateTimeOffset From,
            DateTimeOffset Until,
            string Ticker)
        { }
}