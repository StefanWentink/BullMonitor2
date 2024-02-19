using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.TickerValue.Core.Mappers
{
    public class TipRanksValueResponseMapper
        : IMapper<TipRanksEntity?, TipRanksValueResponse>
    {
        public Task<TipRanksValueResponse> Map(TipRanksEntity? value, CancellationToken cancellationToken)
        {
            var response = value is null
                ? new TipRanksValueResponse()
                : new TipRanksValueResponse(
                    value.Value.Price,

                    value.Value.Buy,
                    value.Value.Hold,
                    value.Value.Sell,
                    value.Value.Consensus,
                    value.Value.PriceTarget,

                    value.Value.BullishSentiments,
                    value.Value.NeutralSentiments,
                    value.Value.BearishSentiments,

                    value.Value.SentimentsScore,
                    value.Value.AverageSentiments);

            return Task.FromResult(response);
        }
    }
}
