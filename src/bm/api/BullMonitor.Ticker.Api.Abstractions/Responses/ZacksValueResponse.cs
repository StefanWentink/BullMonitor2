using BullMonitor.Abstractions.Interfaces;

namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record ZacksValueResponse(
        int Rank,
        int Value,
        int Growth,
        int Momentum,
        int VGM,

        decimal PriceTarget,
        decimal LowestPriceTarget,
        decimal HighestPriceTarget,
        decimal PercentagePriceTarget,

        int StrongBuy,
        int Buy,
        int Hold,
        int Sell,
        int StrongSell,

        decimal Recommendation)
        : IEquatable<ZacksValueResponse>
        , IRank
        , IPriceTargetRange
        , IAdvancedPurchaseAdvice
        , IBrokerRecommendation
    {
        /// <summary>
        /// Constructor used to create an empty/default instance
        /// </summary>
        public ZacksValueResponse()
            : this(
                5, 5, 5, 5, 5,
                0m, 0m, 0m, 0m,
                0, 0, 0, 0, 0,
                0)
        { }
    }
}