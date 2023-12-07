
namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record CompanyResponse(
        Guid Id,
        string Code,
        IDictionary<DateTimeOffset, ValueResponse> Values)
    { }
}