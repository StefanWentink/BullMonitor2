namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record IndustryListResponse(
        Guid Id,
        string Code,
        string Name,
        Guid SectorId)
    { }
}