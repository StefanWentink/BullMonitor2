using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.TickerValue.Core.Mappers
{
    public class ZacksValueResponseMapper
        : IMapper<ZacksEntity?, ZacksValueResponse>
    {
        public Task<ZacksValueResponse> Map(ZacksEntity? value, CancellationToken cancellationToken)
        {
            var response = value is null
                ? new ZacksValueResponse()
                : new ZacksValueResponse(
                    value.Value.Rank,
                    value.Value.Value,
                    value.Value.Growth,
                    value.Value.Momentum,
                    value.Value.VGM,

                    value.Value.PriceTarget,
                    value.Value.LowestPriceTarget,
                    value.Value.HighestPriceTarget,
                    value.Value.PercentagePriceTarget,

                    value.Value.StrongBuy,
                    value.Value.Buy,
                    value.Value.Hold,
                    value.Value.Sell,
                    value.Value.StrongSell,

                    value.Value.Recommendation);

            return Task.FromResult(response);
        }
    }
}
