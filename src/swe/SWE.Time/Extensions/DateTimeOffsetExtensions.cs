using SWE.Time.Enum;
using SWE.Extensions.Extensions;
using System.Collections.Concurrent;
using Ardalis.GuardClauses;
using SWE.Time.Utilities;

namespace SWE.Time.Extensions
{
    public static  class DateTimeOffsetExtensions
    {
        private static readonly ConcurrentDictionary<TimeGranularity, TimeSpan> TimeSpanByGranularity =
            new()
            {
                [TimeGranularity.Second] = TimeSpan.FromSeconds(1),
                [TimeGranularity.Minute] = TimeSpan.FromMinutes(1),
                [TimeGranularity.Quarter] = TimeSpan.FromMinutes(15),
                [TimeGranularity.Hour] = TimeSpan.FromHours(1),
                [TimeGranularity.Day] = TimeSpan.FromDays(1),
                [TimeGranularity.Week] = TimeSpan.FromDays(7), //FromWeek(1) gets normalized to FromDays(7)
            };

        public static TimeSpan ToTimeSpan(this TimeGranularity granularity)
        {
            if (TimeSpanByGranularity.TryGetValue(granularity, out var timeSpan))
            {
                return timeSpan;
            }

            throw new ArgumentOutOfRangeException(
                nameof(granularity),
                granularity,
                $"{nameof(DateTimeOffsetExtensions)}.{nameof(ToTimeSpan)} does not support {nameof(granularity)} {granularity}.");
        }

        public static DateTimeOffset Subtract(
           this DateTimeOffset value,
           TimeGranularity granularity,
           TimeZoneInfo? timeZoneInfo = null)
        {
            return granularity switch
            {
                TimeGranularity.Second => value.Add(seconds: -1, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Minute => value.Add(minutes: -1, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Quarter => value.Add(minutes: -15, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Hour => value.Add(hours: -1, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Day => value.Add(days: -1, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Week => value.Add(days: -7, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Month => value.Add(months: -1, timeZoneInfo: timeZoneInfo),
                TimeGranularity.QuarterYear => value.Add(months: -3, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Year => value.Add(years: -1, timeZoneInfo: timeZoneInfo),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(granularity),
                    granularity,
                    $"{nameof(DateTimeOffsetExtensions)}.{nameof(Subtract)} does not support {nameof(granularity)} {granularity}.")
            };
        }

        public static Func<DateTimeOffset, DateTimeOffset> GetDateSelector(
            this TimeGranularity granularity,
            TimeZoneInfo timeZoneInfo)
        {
            return granularity switch
            {
                TimeGranularity.Second => (DateTimeOffset x) => x.RoundDown(granularity, timeZoneInfo),
                TimeGranularity.Minute => (DateTimeOffset x) => x.RoundDown(granularity, timeZoneInfo),
                TimeGranularity.Quarter => (DateTimeOffset x) => x.RoundDown(granularity, timeZoneInfo),
                TimeGranularity.Hour => (DateTimeOffset x) => x.RoundDown(granularity, timeZoneInfo),
                TimeGranularity.Day => (DateTimeOffset x) => x.SetToStartOfDay(timeZoneInfo),
                TimeGranularity.Week => (DateTimeOffset x) => x.SetToStartOfWeek(timeZoneInfo),
                TimeGranularity.Month => (DateTimeOffset x) => x.SetToStartOfMonth(timeZoneInfo),
                TimeGranularity.QuarterYear => (DateTimeOffset x) => x.SetToStartOfQuarterYear(timeZoneInfo),
                TimeGranularity.Year => (DateTimeOffset x) => x.SetToStartOfYear(timeZoneInfo),
                _ => throw new InvalidOperationException(
                                        $"{nameof(DateTimeOffsetExtensions)}.{nameof(GetDateSelector)} {nameof(TimeGranularity)} '{granularity}' is not valid."),
            };
        }

        public static DateTimeOffset RoundDown(
            this DateTimeOffset value,
            TimeGranularity granularity,
            TimeZoneInfo timeZoneInfo)
        {
            if (granularity.Equals(TimeGranularity.Day))
            {
                var response = value.ToTimeZone(timeZoneInfo);

                if (response.TimeOfDay.Equals(TimeSpan.Zero))
                {
                    return response;
                }

                return response.SetToStartOfDay(timeZoneInfo);
            }

            return value
                .RoundDown(
                    granularity.ToTimeSpan(),
                    timeZoneInfo);
        }

        public static DateTimeOffset RoundUp(
            this DateTimeOffset value,
            TimeGranularity granularity,
            TimeZoneInfo timeZoneInfo)
        {
            if (granularity.Equals(TimeGranularity.Day))
            {
                var response = value.ToTimeZone(timeZoneInfo);

                if (response.TimeOfDay.Equals(TimeSpan.Zero))
                {
                    return response;
                }

                return response.AddDays(1).SetToStartOfDay(timeZoneInfo);
            }

            return value.RoundUp(
                  granularity.ToTimeSpan(),
                  timeZoneInfo);
        }
        public static IEnumerable<DateTimeOffset> IntersectByGranularity(
            this DateTimeOffset from,
            DateTimeOffset until,
            TimeGranularity granularity,
            TimeZoneInfo? timeZoneInfo = null,
            bool respectRange = false)
        {
            Guard
                .Against
                .InvalidInput(
                    (from, until),
                    nameof(from),
                    (x) => x.until >= x.from,
                    $"{nameof(DateTimeOffsetExtensions)}.{nameof(IntersectByGranularity)}: {nameof(until)} should be greater or equal to {nameof(from)}.");

            var zone = timeZoneInfo ?? TimeZoneInfoUtilities.DutchTimeZoneInfo;

            var reference = (granularity < TimeGranularity.Day)
                ? from.RoundDown(granularity, zone)
                : GetDateSelector(granularity, zone)(from);

            if (respectRange && reference < from)
            {
                yield return from;
            }
            else
            {
                yield return reference;
            }

            reference = reference.Add(granularity, zone);

            while (reference < until)
            {
                yield return reference;

                reference = reference.Add(granularity, zone);
            }
        }

        public static DateTimeOffset Add(
           this DateTimeOffset value,
           TimeGranularity granularity,
           TimeZoneInfo? timeZoneInfo = null,
           int amount = 1)
        {
            return granularity switch
            {
                TimeGranularity.Second => value.Add(seconds: amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Minute => value.Add(minutes: amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Quarter => value.Add(minutes: 15 * amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Hour => value.Add(hours: amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Day => value.Add(days: amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Week => value.Add(days: 7 * amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Month => value.Add(months: amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.QuarterYear => value.Add(months: 3 * amount, timeZoneInfo: timeZoneInfo),
                TimeGranularity.Year => value.Add(years: amount, timeZoneInfo: timeZoneInfo),
                _ => throw new ArgumentOutOfRangeException(
                                        nameof(granularity),
                                        granularity,
                                        $"{nameof(DateTimeOffsetExtensions)}.{nameof(DateTimeOffsetExtensions.Add)}: {nameof(granularity)} '{granularity}' is out of range."),
            };
        }
    }
}