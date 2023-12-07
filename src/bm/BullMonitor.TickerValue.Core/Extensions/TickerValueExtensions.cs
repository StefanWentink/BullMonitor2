using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;

namespace BullMonitor.TickerValue.Core.Extensions
{
    public static class TickerValueExtensions
    {
        public static IDictionary<DateTimeOffset, ValueResponse> ToValueDictionary(
            this IEnumerable<ZacksEntity> zacks,
            IEnumerable<TipRanksEntity> tipranks)
        {
            return zacks
                .ToValues(tipranks)
                .ToDictionary(x => x.date, x => x.value);
        }

        public static IEnumerable<(DateTimeOffset date, ValueResponse value)> ToValues(
            this IEnumerable<ZacksEntity> zacks,
            IEnumerable<TipRanksEntity> tipranks)
        {
            var dates = zacks.Select(x => x.ReferenceDate).ToList();
            dates.AddRange(tipranks.Select(x => x.ReferenceDate));

            foreach (var date in dates.Distinct())
            {
                var z = zacks.FirstOrDefault(x => x.ReferenceDate.Equals(date));
                var t = tipranks.FirstOrDefault(x => x.ReferenceDate.Equals(date));

                yield return (date, z.ToValueResponse(t));
            }
        }

        public static ValueResponse ToValueResponse(
            this ZacksEntity? zacks,
            TipRanksEntity? tipranks)
        {
            if (zacks is null)
            {
                if (tipranks is null)
                {
                    return new ValueResponse();
                }

                return new ValueResponse(
                    tipranks.Value.Price,

                    tipranks.Value.Buy,
                    tipranks.Value.Hold,
                    tipranks.Value.Sell,
                    tipranks.Value.Consensus,
                    tipranks.Value.PriceTarget,

                    tipranks.Value.BullishSentiments,
                    tipranks.Value.NeutralSentiments,
                    tipranks.Value.BearishSentiments,

                    tipranks.Value.SentimentsScore,
                    tipranks.Value.AverageSentiments);
            }

            if (tipranks is null)
            {
                return new ValueResponse(
                    zacks.Value.Rank,
                    zacks.Value.Value,
                    zacks.Value.Growth,
                    zacks.Value.Momentum,
                    zacks.Value.VGM,

                    zacks.Value.AveragePriceTarget,
                    zacks.Value.LowestPriceTarget,
                    zacks.Value.HighestPriceTarget,
                    zacks.Value.PercentagePriceTarget,

                    zacks.Value.StrongBuy,
                    zacks.Value.Buy,
                    zacks.Value.Hold,
                    zacks.Value.Sell,
                    zacks.Value.StrongSell,

                    zacks.Value.AverageBrokerRecommendation);
            }

            return new ValueResponse(
                tipranks.Value.Price,

                tipranks.Value.Buy,
                tipranks.Value.Hold,
                tipranks.Value.Sell,
                tipranks.Value.Consensus,
                tipranks.Value.PriceTarget,

                tipranks.Value.BullishSentiments,
                tipranks.Value.NeutralSentiments,
                tipranks.Value.BearishSentiments,

                tipranks.Value.SentimentsScore,
                tipranks.Value.AverageSentiments,

                zacks.Value.Rank,
                zacks.Value.Value,
                zacks.Value.Growth,
                zacks.Value.Momentum,
                zacks.Value.VGM,

                zacks.Value.AveragePriceTarget,
                zacks.Value.LowestPriceTarget,
                zacks.Value.HighestPriceTarget,
                zacks.Value.PercentagePriceTarget,

                zacks.Value.StrongBuy,
                zacks.Value.Buy,
                zacks.Value.Hold,
                zacks.Value.Sell,
                zacks.Value.StrongSell,

                zacks.Value.AverageBrokerRecommendation);
        }
    }
}
