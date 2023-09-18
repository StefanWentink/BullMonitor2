namespace BullMonitor.Abstractions.Utilities
{
    public static class ZacksUtilities
    {
        public static decimal GetScore(
            int rank,
            int value,
            int growth,
            int momentum,
            int vgm)
        {
            return 100
                - ((rank - 1) * 20)
                - ((value - 1) * 1)
                - ((growth - 1) * 1)
                - ((momentum - 1) * 1)
                - ((vgm - 1) * 2);
        }

        public static decimal GetScore(
            decimal averagePriceTarget,
            decimal highestPriceTarget,
            decimal lowestPriceTarget,
            decimal priceTargetPercentage)
        {
            var baseValue = averagePriceTarget < 5
                ? 10
                : averagePriceTarget < 10
                    ? 25
                    : averagePriceTarget < 20
                        ? 40
                        : 50;

            var result = baseValue
                + (priceTargetPercentage * (1 + baseValue / 100));

            return GetZeroToHundredScore(result);
        }

        public static decimal GetScore(
            int strongBuy,
            int buy,
            int hold,
            int sell,
            int strongSell,
            decimal averageBrokerRecommendation)
        {
            var result = 100 - averageBrokerRecommendation * 20
                + (strongBuy * 2)
                + (buy * 1)
                + (hold * -1);

            result = GetZeroToHundredScore(result);

            result = result
                + (sell * -2)
                + (strongSell * -4);

            return GetZeroToHundredScore(result);
        }

        public static decimal GetScore(
            int buy,
            int hold,
            int sell,
            int consensus,
            decimal pricetarget)
        {
            decimal basevalue = consensus == 0
                ? 45
                : (consensus - 1) * 15;

            var result = basevalue
                + (buy * 1)
                + (hold * -1);

            result = GetZeroToHundredScore(result);

            result = result
                + (sell * -2);

            return GetZeroToHundredScore(result);
        }

        public static decimal GetScore(
            decimal returnOnAssets,
            decimal sixMonthsMomentum,
            decimal volatilityLevel,
            int score,
            decimal returnOnEquity,
            int volatilityLevelRating,
            decimal twelveMonthsMomentum,
            decimal simpleMovingAverage,
            decimal assetGrowth)
        {
            decimal basevalue = score == 0
                ? 45
                : score * 10;

            return GetZeroToHundredScore(basevalue);
        }

        public static decimal GetZeroToHundredScore(
            decimal value)
        {
            return value < 0
                ? 0
                : value > 100
                    ? 100
                    : value;
        }
    }
}