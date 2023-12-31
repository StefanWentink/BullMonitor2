﻿namespace SWE.Extensions.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Interprets and converts a <see cref="DateTime"/> as <see cref="TimeZoneInfo.Local"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffsetLocal(this DateTime value)
        {
            return value.ToDateTimeOffset(TimeZoneInfo.Local);
        }

        /// <summary>
        /// Interprets and converts a <see cref="DateTime"/> as <see cref="TimeZoneInfo.Local"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffsetUtc(this DateTime value)
        {
            return value.ToDateTimeOffset(TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Interprets and converts a <see cref="DateTime"/> as <see cref="timeZoneInfo"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime value, TimeZoneInfo timeZoneInfo)
        {
            return value.ToDateTimeOffset(timeZoneInfo.GetUtcOffset(value));
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to an <see cref="DateTimeOffset"/> with a specific <see cref="TimeSpan"/> offset.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime value, TimeSpan timeSpan)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                return new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, timeSpan);
            }
            return new DateTimeOffset(value, timeSpan);
        }

        /// <summary>
        /// Transfers a <see cref="DateTime"/> from <see cref="timespan"/> to <see cref="timeZoneInfo"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timespan"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(
            this DateTime value,
            TimeSpan timespan,
            TimeZoneInfo timeZoneInfo)
        {
            return new DateTimeOffset(value, timespan).ToTimeZone(timeZoneInfo);
        }

        /// <summary>
        /// Transfers a <see cref="DateTime"/> from <see cref="sourceTimeZone"/> to <see cref="destinationTimeZone"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sourceTimeZone"></param>
        /// <param name="destinationTimeZone"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(
            this DateTime value,
            TimeZoneInfo sourceTimeZone,
            TimeZoneInfo destinationTimeZone)
        {
            var dateTimeOffset = value.ToDateTimeOffset(sourceTimeZone);
            return TimeZoneInfo.ConvertTime(dateTimeOffset, destinationTimeZone);
        }

        internal static IEnumerable<TimeZoneInfo.AdjustmentRule> GetAdjustmentRules(
            this TimeZoneInfo timeZoneInfo,
            DateTimeOffset referenceDate)
        {
            var results = timeZoneInfo.GetAdjustmentRules().ToList();

            return results.Where(
                x =>
                    x.DateStart <= referenceDate.Date
                    && x.DateEnd > referenceDate.Date).ToList();
        }

        internal static TimeZoneInfo.AdjustmentRule? GetAdjustmentRule(
            this TimeZoneInfo timeZoneInfo,
            DateTimeOffset referenceDate)
        {
            var adjustmentRules = timeZoneInfo.GetAdjustmentRules(referenceDate).ToList();

            if (adjustmentRules.Count > 0)
            {
                return adjustmentRules[0];
            }

            return null;
        }

        internal static TimeSpan GetAdjustmentRuleTimeSpan(
            this TimeZoneInfo timeZoneInfo,
            DateTimeOffset referenceDate)
        {
            return timeZoneInfo.GetAdjustmentRule(referenceDate)?.DaylightDelta ?? TimeSpan.Zero;
        }

        /// <summary>
        /// Gets the age of <see cref="value"/> compared to <see cref="compare"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static int GetAge(this DateTime value, DateTime compare)
        {
            var result = compare.Year - value.Year;

            if (compare < value.AddYears(result))
            {
                result--;
            }

            return result;
        }

        public static DateTime ParseDateTimeWithoutSeconds(this string dateTime)
        {
            return DateTime.ParseExact(dateTime, "d-M-yyyy HH:mm", null);
        }

        public static bool MonthDifferenceAtLeast(
            this DateTime from,
            DateTime until,
            int numberOfMonths)
        {
            return from.AddMonths(numberOfMonths) < until;
        }
    }
}
