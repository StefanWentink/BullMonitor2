using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Core.Mappers
{
    public class CompanyListResponseMapper
        : IMapper<TickerEntity, CompanyListResponse>
    {
        public Task<CompanyListResponse> Map(
            TickerEntity value,
            CancellationToken cancellationToken)
        {
            var response = new CompanyListResponse(
                value.Id,
                value.Code,
                value.Name,
                value.CurrencyId,
                value.ExchangeId,
                value.IndustryId,
                value.KnownByZacks,
                value.KnownByTipranks);

            return Task.FromResult( response );
        }
    }
}
