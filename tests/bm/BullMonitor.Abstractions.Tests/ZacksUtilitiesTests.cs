using BullMonitor.Abstractions.Utilities;
using FluentAssertions;
using Xunit;

namespace BullMonitor.Abstractions.Tests
{
    public class ZacksUtilitiesTests
    {
        [Theory]
        [InlineData(3, 3, 3, 3, 3, 50)]
        [InlineData(1, 1, 1, 1, 1, 100)]
        [InlineData(1, 1, 1, 1, 5, 92)]
        [InlineData(1, 5, 1, 1, 1, 96)]
        [InlineData(1, 1, 1, 5, 5, 88)]
        [InlineData(1, 1, 5, 5, 5, 84)]
        [InlineData(1, 5, 5, 5, 5, 80)]
        [InlineData(5, 5, 5, 5, 5, 0)]
        public void GetScore_Should_Calculate_Score_With_ZacksRank(
            int rank,
            int value,
            int growth,
            int momentum,
            int vgm,
            decimal expected)
        {
            var actual = ZacksUtilities
                .GetScore(
                    rank,
                    value,
                    growth,
                    momentum,
                    vgm);

            actual
                .Should()
                .Be(expected);
        }

        [Theory]
        [InlineData(3, 4, 2, 20, 30)]
        [InlineData(3, 4, 2, 40, 50)]
        [InlineData(3, 4, 2, 60, 70)]
        [InlineData(3, 5, 2, 20, 30)]
        [InlineData(3, 4, 1, 20, 30)]

        [InlineData(6,  8, 4, 20, 45)]
        [InlineData(6,  8, 4, 40, 65)]
        [InlineData(6,  8, 4, 60, 85)]
        [InlineData(6, 10, 4, 20, 45)]
        [InlineData(6,  8, 2, 20, 45)]

        [InlineData(12, 16, 8, 20, 60)]
        [InlineData(12, 16, 8, 40, 80)]
        [InlineData(12, 16, 8, 60, 100)]
        [InlineData(12, 20, 8, 20, 60)]
        [InlineData(12, 16, 4, 20, 60)]

        public void GetScore_Should_Calculate_Score_With_ZacksPriceTarget(
            decimal averagePriceTarget,
            decimal highestPriceTarget,
            decimal lowestPriceTarget,
            decimal priceTargetPercentage,
            decimal expected)
        {
            var actual = ZacksUtilities
                .GetScore(
                    averagePriceTarget,
                    highestPriceTarget,
                    lowestPriceTarget,
                    priceTargetPercentage);

            actual
                .Should()
                .Be(expected);
        }

        [Theory]
        [InlineData( 6, 1, 0, 0, 0, 1.2, 89)]
        [InlineData(10, 2, 1, 2, 4, 2.2, 57)]
        [InlineData(11, 2, 1, 2, 4, 2.2, 59)]
        [InlineData(10, 3, 1, 2, 4, 2.2, 58)]
        [InlineData(10, 2, 2, 2, 4, 2.2, 56)]
        [InlineData(10, 2, 1, 3, 4, 2.2, 55)]
        [InlineData(10, 2, 1, 2, 5, 2.2, 53)]
        [InlineData(10, 2, 1, 2, 4, 3.2, 37)]
        [InlineData( 0, 1, 4, 2, 6, 3.6, 0)]

        public void GetScore_Should_Calculate_Score_With_ZacksRecommendation(
            int strongBuy,
            int buy,
            int hold,
            int sell,
            int strongSell,
            decimal averageBrokerRecommendation,
            decimal expected)
        {
            var actual = ZacksUtilities
                .GetScore(
                    strongBuy,
                    buy,
                    hold,
                    sell,
                    strongSell,
                    averageBrokerRecommendation);

            actual
                .Should()
                .Be(expected);
        }
    }
}