using BullMonitor.Abstractions.Utilities;
using MoreLinq.Extensions;

namespace BullMonitor.Abstractions.Responses
{
    public record ZacksRankResponse(
        string Ticker,
        int Rank,
        string ValueString,
        string GrowthString,
        string MomentumString,
        string VGMString,

        string AveragePriceTargetString,
        string HighestPriceTargetString,
        string LowestPriceTargetString,
        string PriceTargetPercentageString,

        string StrongBuyString,
        string BuyString,
        string HoldString,
        string SellString,
        string StrongSellString,
        string AverageBrokerRecommendationString)
    {
        public ZacksRankResponse(
            string Ticker)
            : this(
                 Ticker,
                 0,
                 "E",
                 "E",
                 "E",
                 "E",
                 "$0.00",
                 "$0.00",
                 "$0.00",
                 "0.00%",
                 "0",
                 "0",
                 "0",
                 "0",
                 "0",
                 "5.00")
        { }

        private int? _value = null;
        private int? _growth = null;
        private int? _momentum = null;
        private int? _vgm = null;

        private decimal? _averagePriceTargetString = null;
        private decimal? _highestPriceTargetString = null;
        private decimal? _lowestPriceTargetString = null;
        private decimal? _priceTargetPercentageString = null;

        private int? _strongBuy = null;
        private int? _buy = null;
        private int? _hold = null;
        private int? _sell = null;
        private int? _strongSell = null;
        private decimal? _averageBrokerRecommendation = null;

        public int Value => _value ??= ToRank(ValueString);
        public int Growth => _growth ??= ToRank(GrowthString);
        public int Momentum => _momentum ??= ToRank(MomentumString);
        public int VGM => _vgm ??= ToRank(VGMString);
        
        public decimal AveragePriceTarget => _averagePriceTargetString ??= StringValueUtilities.ToAmount(AveragePriceTargetString);
        public decimal HighestPriceTarget => _highestPriceTargetString ??= StringValueUtilities.ToAmount(HighestPriceTargetString);
        public decimal LowestPriceTarget => _lowestPriceTargetString ??= StringValueUtilities.ToAmount(LowestPriceTargetString);
        public decimal PriceTargetPercentage => _priceTargetPercentageString ??= StringValueUtilities.ToPercentage(PriceTargetPercentageString);

        public int StrongBuy => _strongBuy ??= StringValueUtilities.ToInteger(StrongBuyString);
        public int Buy => _buy ??= StringValueUtilities.ToInteger(BuyString);
        public int Hold => _hold ??= StringValueUtilities.ToInteger(HoldString);
        public int Sell => _sell ??= StringValueUtilities.ToInteger(SellString);
        public int StrongSell => _strongSell ??= StringValueUtilities.ToInteger(StrongSellString);
        public decimal AverageBrokerRecommendation => _averageBrokerRecommendation ??= StringValueUtilities.ToPercentage(AverageBrokerRecommendationString);

        private static int ToRank(string value)
        {
            return value.Trim() switch
            {
                "a" or "A" => 1,
                "b" or "B" => 2,
                "c" or "C" => 3,
                "d" or "D" => 4,
                _ => 5,
            };
        }

    }
}