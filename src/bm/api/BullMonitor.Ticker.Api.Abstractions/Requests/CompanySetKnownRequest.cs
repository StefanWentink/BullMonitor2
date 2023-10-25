namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record CompanySetKnownRequest(
        string Code,
        bool Known)
    { }
}