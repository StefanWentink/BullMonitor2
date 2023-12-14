using BullMonitor.Data.Storage.Interfaces;
using SWE.Time.Extensions;

namespace BullMonitor.Data.Storage.Models
{
    public record struct ValueEntity(
            Guid TickerId,
            DateTimeOffset ReferenceDate,
            int TipranksConsensus,
            decimal TipranksPriceTarget,
            int TipranksSentimentsScore,
            int ZacksRank,
            decimal ZacksPriceTarget,
            decimal ZacksBrokerRecommendation
        )
        : ITickerValue
    {
        private int? _year = null;
        private int? _week = null;
        private int? _weeksSince = null;
        private SWE.Time.Entities.Range? _range = null;

        public int Year => _year ??= ReferenceDate.Year;
        public int Week => _week ??= ReferenceDate.GetDutchWeekNumber();
        public int YearWeek => Year * 100 + Week;
        public int WeeksSinceStart => _weeksSince ??= ReferenceDate.WeeksSinceStart();
        public SWE.Time.Entities.Range Range => _range ??= ReferenceDate.GetWeekRange();
    }
}