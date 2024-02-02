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
    { }
}