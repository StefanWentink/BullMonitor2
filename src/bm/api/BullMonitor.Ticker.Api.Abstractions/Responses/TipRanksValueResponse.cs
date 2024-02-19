using BullMonitor.Abstractions.Interfaces;

namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record TipRanksValueResponse(
        decimal Price,

        int Buy,
        int Hold,
        int Sell,
        int Consensus,
        decimal PriceTarget,

        int BullishSentiments,
        int NeutralSentiments,
        int BearishSentiments,

        int SentimentsScore,
        decimal AverageSentiments)
        : IEquatable<TipRanksValueResponse>
        , IPrice
        , IBasicPurchaseAdvice
        , IConsensus
        , IPriceTarget
        , ISentiments
    {
        /// <summary>
        /// Constructor used to create an empty/default instance
        /// </summary>
        public TipRanksValueResponse()
            : this(
                0m,

                0, 0, 0, 0, 0m,

                0, 0, 0,

                0, 0m)
        { }
    }
}