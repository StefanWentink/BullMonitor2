using SWE.Time.Enum;
using SWE.Extensions.Extensions;
using System.Collections.Concurrent;
using Ardalis.GuardClauses;
using SWE.Time.Utilities;

namespace SWE.Time.Extensions
{
    public static class DateTimeOffsetExtensions
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

        public static DateTimeOffset Add(
           this DateTimeOffset value,
           int seconds = 0,
           int minutes = 0,
           int hours = 0,
           int days = 0,
           int months = 0,
           int years = 0,
           TimeZoneInfo? timeZoneInfo = null)
        {
            var relevantTimeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;

            var response = value.ToTimeZone(relevantTimeZoneInfo);

            response = seconds == 0 ? response : response.AddSeconds(seconds);
            response = minutes == 0 ? response : response.AddMinutes(minutes);
            response = hours == 0 ? response : response.AddHours(hours);
            response = days == 0 ? response : response.DateTime.AddDays(days).ToDateTimeOffset(relevantTimeZoneInfo);
            response = months == 0 ? response : response.DateTime.AddMonths(months).ToDateTimeOffset(relevantTimeZoneInfo);
            response = years == 0 ? response : response.DateTime.AddYears(years).ToDateTimeOffset(relevantTimeZoneInfo);

            return response.ToTimeZone(relevantTimeZoneInfo);
        }

        /// <summary>
        /// Transfers a <see cref="DateTimeOffset"/> from it's current timezone to <see cref="TimeZoneInfo.Utc"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTimeOffset ToUtcTimeZone(
            this DateTimeOffset value)
        {
            return value.UtcDateTime.ToDateTimeOffset(TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Transfers a <see cref="DateTimeOffset"/> from <see cref="TimeZoneInfo.Utc"/> to <see cref="timeZoneInfo"/>
        /// disregarding it's own offset.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset ToTimeZoneFromUtc(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            return value.ToTimeZone(TimeZoneInfo.Utc, timeZoneInfo);
        }

        /// <summary>
        /// Transfers a <see cref="DateTimeOffset"/> from <see cref="TimeZoneInfo.Local"/> to <see cref="timeZoneInfo"/>
        /// disregarding it's own offset
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset FromLocalToTimeZone(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            return value.ToTimeZone(TimeZoneInfo.Local, timeZoneInfo);
        }

        /// <summary>
        /// Transfers a <see cref="DateTimeOffset"/> from <see cref="sourceTimeZone"/> to <see cref="destinationTimeZone"/>
        /// disregarding it's own offset
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sourceTimeZone"></param>
        /// <param name="destinationTimeZone"></param>
        /// <returns></returns>
        public static DateTimeOffset ToTimeZone(
            this DateTimeOffset value,
            TimeZoneInfo sourceTimeZone,
            TimeZoneInfo destinationTimeZone)
        {
            return value.DateTime.ToDateTimeOffset(sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        /// Transfers a <see cref="DateTimeOffset"/> from it's current timezone to <see cref="timeZoneInfo"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset ToTimeZone(
            this DateTimeOffset value,
            TimeZoneInfo? timeZoneInfo)
        {
            var relevantTimeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;

            return value
                .ToUtcTimeZone()
                .ToTimeZoneFromUtc(relevantTimeZoneInfo);
        }

        /// <summary>
        /// Set the time of day to 00:00:00.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset SetToStartOfDay(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            return value
                .SetToTimeOfDay(0, 0, 0, timeZoneInfo);
        }

        public static int StartOfWeekDayDelta(
            this DayOfWeek value)
        {
            return value switch
            {
                DayOfWeek.Sunday => -6,
                DayOfWeek.Monday => 0,
                DayOfWeek.Tuesday => -1,
                DayOfWeek.Wednesday => -2,
                DayOfWeek.Thursday => -3,
                DayOfWeek.Friday => -4,
                DayOfWeek.Saturday => -5,
                _ => 0,
            };
        }

        public static DateTimeOffset SetToStartOfWeek(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            var response = value
                .SetToStartOfDay(timeZoneInfo);

            var startOfWeekDayDelta = StartOfWeekDayDelta(response.DayOfWeek);

            if (startOfWeekDayDelta == 0)
            {
                return response;
            }

            return response
                .Add(days: startOfWeekDayDelta, timeZoneInfo: timeZoneInfo);
        }

        public static DateTimeOffset SetToStartOfMonth(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            var response = value
                .SetToStartOfDay(timeZoneInfo);

            var dayOfMonthDelta = (response.Day - 1);

            if (dayOfMonthDelta == 0)
            {
                return response;
            }

            return response
                .Add(days: -dayOfMonthDelta, timeZoneInfo: timeZoneInfo);
        }

        public static DateTimeOffset SetToStartOfQuarterYear(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            var response = value
                .SetToStartOfDay(timeZoneInfo);

            var dayOfMonthDelta = (response.Day - 1);
            var monthDelta = (response.Month - 1) % 3;

            if (dayOfMonthDelta == 0 && monthDelta == 0)
            {
                return response;
            }

            return response
                .Add(days: -dayOfMonthDelta, months: -monthDelta, timeZoneInfo: timeZoneInfo);
        }

        public static DateTimeOffset SetToStartOfYear(
            this DateTimeOffset value,
            TimeZoneInfo timeZoneInfo)
        {
            var response = value
                .SetToStartOfDay(timeZoneInfo);

            var dayOfMonthDelta = (response.Day - 1);
            var monthDelta = response.Month - 1;

            if (dayOfMonthDelta == 0 && monthDelta == 0)
            {
                return response;
            }

            return response
                .Add(days: -dayOfMonthDelta, months: -monthDelta, timeZoneInfo: timeZoneInfo);
        }

        /// <summary>
        /// Set the time of day to a <paramref name="hour"/>:<paramref name="minute"/>:<paramref name="second"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset SetToTimeOfDay(
            this DateTimeOffset value,
            int hour,
            int minute,
            int second,
            TimeZoneInfo timeZoneInfo)
        {
            var response = value.ToTimeZone(timeZoneInfo);

            if (response.Hour == hour
                && response.Minute == 0
                && response.Second == 0
                && response.Millisecond == 0)
            {
                return response;
            }

            return new DateTime(response.Year, response.Month, response.Day, hour, minute, second)
                .ToDateTimeOffset(timeZoneInfo);
        }
        /// <summary>
        /// Sets the time to the nearest factor of <paramref name="timeSpan"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset Round(
            this DateTimeOffset value,
            TimeSpan timeSpan,
            TimeZoneInfo? timeZoneInfo)
        {
            return value.DoRound(timeSpan, timeZoneInfo, null);
        }

        /// <summary>
        /// Sets the time to the previous factor of <paramref name="timeSpan"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset RoundDown(
            this DateTimeOffset value,
            TimeSpan timeSpan,
            TimeZoneInfo? timeZoneInfo)
        {
            return value
                .DoRound(
                    timeSpan,
                    timeZoneInfo,
                    false);
        }

        /// <summary>
        /// Sets the time to the next factor of <see cref="timeSpan"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset RoundUp(
            this DateTimeOffset value,
            TimeSpan timeSpan,
            TimeZoneInfo? timeZoneInfo)
        {
            return value
                .DoRound(
                    timeSpan,
                    timeZoneInfo,
                    true);
        }

        /// <summary>
        /// Sets the time to the previous factor of <see cref="timeSpan"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <param name="timeZoneInfo"></param>
        /// <param name="roundUp"></param>
        /// <returns></returns>
        private static DateTimeOffset DoRound(
            this DateTimeOffset value,
            TimeSpan timeSpan,
            TimeZoneInfo? timeZoneInfo,
            bool? roundUp = null)
        {
            if (timeSpan >= TimeSpan.FromDays(1))
            {
                throw new NotSupportedException($"No rounding support for {nameof(TimeSpan)} of 1 day or greater.");
            }

            var relevantTimeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;
            var response = value.ToTimeZone(relevantTimeZoneInfo);

            if (timeSpan.Ticks == 0)
            {
                return response;
            }

            var (previous, next) = response.GetDeltaTicks(timeSpan);

            if (next == 0 || previous == 0)
            {
                return response;
            }

            return roundUp ?? (next <= previous)
                    ? (next == 0
                        ? response
                        : response.AddTicks(next).ToTimeZone(relevantTimeZoneInfo))
                    : (previous == 0
                        ? response
                        : response.AddTicks(-previous).ToTimeZone(relevantTimeZoneInfo));
        }

        /// <summary>
        /// Sets the time to the previous factor of <see cref="timeSpan"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private static (long previous, long next) GetDeltaTicks(
            this DateTimeOffset value,
            TimeSpan timeSpan)
        {
            if (timeSpan.Ticks == 0)
            {
                return (0, 0);
            }

            var ticks = value.Ticks;

            // delta to previous 'round' time.          PreviousRoundTime <- previous <- Now
            var previous = ticks % timeSpan.Ticks;
            // delta to next 'round' time.                                               Now -> next -> NextRoundTime
            var next = timeSpan.Ticks - previous;

            return (previous, next);
        }

        public static bool MonthDifferenceAtLeast(
           this DateTimeOffset from,
           DateTimeOffset until,
           int numberOfMonths)
        {
            return from.AddMonths(numberOfMonths) < until;
        }
    }
}