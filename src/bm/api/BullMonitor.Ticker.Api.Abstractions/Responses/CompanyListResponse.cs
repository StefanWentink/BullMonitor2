namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record CompanyListResponse(
        Guid Id,
        string Code,
        string Name,
        Guid CurrencyId,
        Guid ExchangeId,
        Guid IndustryId,
        bool? KnownByZacks,
        bool? KnownByTipRanks)
    { }
}