using SWE.Time.Extensions;
using SWE.Time.Utilities;

namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    internal static class CompanyValueRequestDateGenerator
    {
        internal static DateTimeOffset From => new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.FromHours(1));
        internal static DateTimeOffset Until => DateTimeOffset
            .Now
            .Add(days: 1, timeZoneInfo: TimeZoneInfoUtilities.DutchTimeZoneInfo)
            .SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo);
    }

    public record CompanyValueRequest(
        string Code,
        DateTimeOffset From,
        DateTimeOffset Until,
        bool? KnownByZacks,
        bool? KnownByTipRanks)
    {
        public CompanyValueRequest(
            string Code)
            : this(
                Code,
                CompanyValueRequestDateGenerator.From,
                CompanyValueRequestDateGenerator.Until,
                null,
                null)
        { }

        public CompanyValueRequest(
            string Code,
            DateTimeOffset From,
            DateTimeOffset Until)
            : this(
                Code,
                From,
                Until,
                null,
                null)
        { }

        public CompanyValueRequest(
            DateTimeOffset From,
            DateTimeOffset Until,
            bool KnownByZacks,
            bool KnownByTipRanks)
            : this(
                string.Empty,
                From,
                Until,
                KnownByZacks,
                KnownByTipRanks)
        { }

        public CompanyValueRequest(
            bool KnownByZacks,
            bool KnownByTipRanks)
            : this(
                string.Empty,
                CompanyValueRequestDateGenerator.From,
                CompanyValueRequestDateGenerator.Until,
                KnownByZacks,
                KnownByTipRanks)
        { }
    }
}