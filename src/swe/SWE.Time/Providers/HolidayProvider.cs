using Microsoft.Extensions.Logging;
using SWE.Time.Extensions;
using SWE.Time.Interfaces;
using SWE.Time.Utilities;

namespace SWE.Time.Providers
{
    public class HolidayProvider
        : IHolidayProvider
    {
        private static readonly SemaphoreSlim _holidayDictionaryLock = new(1);

        private Dictionary<int, IEnumerable<DateTimeOffset>> HolidayByYearDictionary { get; } = new();

        protected ILogger<HolidayProvider> Logger { get; }

        public HolidayProvider(ILogger<HolidayProvider> logger)
        {
            Logger = logger;
        }

        public Task<IEnumerable<DateTimeOffset>> Get(
            int key,
            CancellationToken cancellationToken)
        {
            return Get((key, TimeZoneInfoUtilities.DutchTimeZoneInfo), cancellationToken);
        }

        public Task<IEnumerable<DateTimeOffset>> Get(
            (int year, TimeZoneInfo timeZoneInfo) key,
            CancellationToken cancellationToken)
        {
            var response = ChristianHolidays(key.year, key.timeZoneInfo).ToList();

            response.Add(KingsDay(key.year, key.timeZoneInfo));
            response.Add(NewYear(key.year, key.timeZoneInfo));
            response.AddRange(Christmas(key.year, key.timeZoneInfo));

            return Task.FromResult<IEnumerable<DateTimeOffset>>(response);
        }

        internal static DateTimeOffset NewYear(
            int year,
            TimeZoneInfo timeZoneInfo)
        {
            return new DateTime(year, 1, 1).ToDateTimeOffset(timeZoneInfo);
        }

        internal static DateTimeOffset KingsDay(
            int year,
            TimeZoneInfo timeZoneInfo)
        {
            // kings day
            var response = new DateTime(year, 4, 27).ToDateTimeOffset(timeZoneInfo);

            if (year <= 2013)
            {
                // queens day
                response = new DateTime(year, 4, 30).ToDateTimeOffset(timeZoneInfo);
            }

            return response.DayOfWeek == DayOfWeek.Sunday
                ? year < 1980
                    ? response.Add(days: 1, timeZoneInfo: timeZoneInfo)     // Maandag erna
                    : response.Add(days: -1, timeZoneInfo: timeZoneInfo)    // Zaterdag ervoor
                : response;
        }

        internal static IEnumerable<DateTimeOffset> ChristianHolidays(
            int year,
            TimeZoneInfo timeZoneInfo)
        {
            var easterSunday = EasterSunday(year, timeZoneInfo);

            yield return easterSunday;
            yield return easterSunday.Add(days: 1, timeZoneInfo: timeZoneInfo); // 2e paasdag

            yield return easterSunday.Add(days: 39, timeZoneInfo: timeZoneInfo); // Hemelvaart

            yield return easterSunday.Add(days: 49, timeZoneInfo: timeZoneInfo); // 1e Pinksterdag
            yield return easterSunday.Add(days: 50, timeZoneInfo: timeZoneInfo); // 2e Pinksterdag
        }

        internal static DateTimeOffset EasterSunday(
            int year,
            TimeZoneInfo timeZoneInfo)
        {
            int g = year % 19;
            int century = year / 100;
            int h = (century - (int)(century / 4) - (int)((8 * century + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            int day = i - (year + year / 4 + i + 2 - century + century / 4) % 7 + 28;
            int month = 3;
            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day).ToDateTimeOffset(timeZoneInfo);
        }

        internal static IEnumerable<DateTimeOffset> Christmas(
            int year,
            TimeZoneInfo timeZoneInfo)
        {
            yield return new DateTime(year, 12, 25).ToDateTimeOffset(timeZoneInfo);
            yield return new DateTime(year, 12, 26).ToDateTimeOffset(timeZoneInfo);
        }

        public async Task<bool> GetSingleOrDefault(
            DateTimeOffset key,
            CancellationToken cancellationToken)
        {
            await _holidayDictionaryLock
                .WaitAsync(30_000)
                .ConfigureAwait(false);

            try
            {
                if (!HolidayByYearDictionary.ContainsKey(key.Year))
                {
                    var holidays = await Get((key.Year, TimeZoneInfoUtilities.DutchTimeZoneInfo), cancellationToken)
                        .ConfigureAwait(false);

                    HolidayByYearDictionary.Add(key.Year, holidays);

                    return holidays.Contains(key);
                }

                return HolidayByYearDictionary[key.Year]
                    .Contains(key);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception.Message);
                throw;
            }
            finally
            {
                _holidayDictionaryLock.Release();
            }
        }
    }
}