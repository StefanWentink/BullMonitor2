namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record ValueResponse(
        decimal Price,

        int TipRanksBuy,
        int TipRanksHold,
        int TipRanksSell,
        int Consensus,
        decimal TipRanksPriceTarget,

        int BullishSentiments,
        int NeutralSentiments,
        int BearishSentiments,

        int SentimentsScore,
        decimal AverageSentiments,

        int Rank,
        int Value,
        int Growth,
        int Momentum,
        int VGM,

        decimal ZacksAveragePriceTarget,
        decimal ZacksLowestPriceTarget,
        decimal ZacksHighestPriceTarget,
        decimal ZacksPercentagePriceTarget,

        int ZacksStrongBuy,
        int ZacksBuy,
        int ZacksHold,
        int ZacksSell,
        int ZacksStrongSell,

        decimal ZacksAverageBrokerRecommendation)
        : IEquatable<ValueResponse>
    {
        public ValueResponse(
            decimal Price,

            int TipRanksBuy,
            int TipRanksHold,
            int TipRanksSell,
            int Consensus,
            decimal TipRanksPriceTarget,

            int BullishSentiments,
            int NeutralSentiments,
            int BearishSentiments,

            int SentimentsScore,
            decimal AverageSentiments)
            : this(
                Price,
                TipRanksBuy,
                TipRanksHold,
                TipRanksSell,
                Consensus,
                TipRanksPriceTarget,

                BullishSentiments,
                NeutralSentiments,
                BearishSentiments,

                SentimentsScore,
                AverageSentiments,

                5, 5, 5, 5, 5,

                0m, 0m, 0m, 0m,

                0, 0, 0, 0, 0,

                0)
        { }

        public ValueResponse(
            int Rank,
            int Value,
            int Growth,
            int Momentum,
            int VGM,

            decimal ZacksAveragePriceTarget,
            decimal ZacksLowestPriceTarget,
            decimal ZacksHighestPriceTarget,
            decimal ZacksPercentagePriceTarget,

            int ZacksStrongBuy,
            int ZacksBuy,
            int ZacksHold,
            int ZacksSell,
            int ZacksStrongSell,

            decimal ZacksAverageBrokerRecommendation)
            : this(
                0m,

                0, 0, 0, 0, 0m,

                0, 0, 0,

                0, 0m,

                Rank,
                Value,
                Growth,
                Momentum,
                VGM,

                ZacksAveragePriceTarget,
                ZacksLowestPriceTarget,
                ZacksHighestPriceTarget,
                ZacksPercentagePriceTarget,

                ZacksStrongBuy,
                ZacksBuy,
                ZacksHold,
                ZacksSell,
                ZacksStrongSell,

                ZacksAverageBrokerRecommendation)
        { }

        public ValueResponse()
            : this(
                0m,

                0, 0, 0, 0, 0m,

                0, 0, 0,

                0, 0m,

                5, 5, 5, 5, 5,

                0m, 0m, 0m, 0m,

                0, 0, 0, 0, 0,

                0)
        { }
    }
}