﻿using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.Mappers
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
                value.IndustryId );

            return Task.FromResult( response );
        }
    }
}
