namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record SectorListResponse(
        Guid Id,
        string Code,
        string Name)
    { }
}