using NodaTime.TimeZones;
using NodaTime.TimeZones.Cldr;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace SWE.Time.Utilities
{
    public static class TimeZoneInfoUtilities
    {
        private static TimeZoneInfo? _dutchTimeZoneInfo;

        private static ConcurrentDictionary<string, TimeZoneInfo>? _timeZoneInfoDictionary;

        public static TimeZoneInfo DutchTimeZoneInfo => _dutchTimeZoneInfo ??= GetDutchTimeZoneInfo();

        public static ConcurrentDictionary<string, TimeZoneInfo> TimeZoneInfoDictionary => _timeZoneInfoDictionary ??=
            new ConcurrentDictionary<string, TimeZoneInfo>(
                TimeZoneInfo
                    .GetSystemTimeZones()
                    .ToDictionary(x => x.Id, x => x));

        /// <summary>
        /// Gets <see cref="TimeZoneInfo"/> from SystemTimeZones
        /// </summary>
        /// <param name="timeZoneId">Can be fetched from a <see cref="MapZone"/>.WindowsId. </param>
        /// <returns>Returns <see cref="TimeZoneInfo.Utc"/> if <paramref name="timeZoneId"/> not found.</returns>
        public static TimeZoneInfo GetTimeZoneInfoByTimeZoneId(
            string timeZoneId)
        {
            return GetTimeZoneInfoByTimeZoneId(timeZoneId, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// Gets <see cref="TimeZoneInfo"/> from SystemTimeZones
        /// </summary>
        /// <param name="timeZoneId">Can be fetched from a <see cref="MapZone"/>.WindowsId.</param>
        /// <param name="defaultTimeZoneInfo"></param>
        /// <returns>Returns <see cref="TimeZoneInfo.Utc"/> if <see cref="timeZoneId"/> not found.</returns>
        private static TimeZoneInfo GetTimeZoneInfoByTimeZoneId(
            string timeZoneId,
            TimeZoneInfo defaultTimeZoneInfo)
        {
            if (TimeZoneInfoDictionary.TryGetValue(timeZoneId, out var _timeZoneInfo))
            {
                return _timeZoneInfo;
            }

            return defaultTimeZoneInfo;
        }

        /// <summary>
        /// Gets <see cref="TimeZoneInfo"/> from SystemTimeZones
        /// </summary>
        /// <param name="countryCode">2 letter countryCode</param>
        /// <returns></returns>
        /// <returns></returns>
        /// <exception cref="ArgumentException">No results in collection for countryCode</exception>
        /// <exception cref="ArgumentException">Multiple results in collection for countryCode</exception>
        private static TimeZoneInfo GetTimeZoneInfoByCountryCode(string countryCode)
        {
            return Enumerable
                .DistinctBy(GetTimeZoneInfosByCountryCode(countryCode), x => x.BaseUtcOffset)
                .ToList()
                .ValidateSingleResultForCountryCode(countryCode);
        }

        /// <summary>
        /// Gets a list of <see cref="TimeZoneInfo"/> from SystemTimeZones
        /// </summary>
        /// <param name="countryCode">2 letter countryCode</param>
        /// <returns></returns>
        private static IEnumerable<TimeZoneInfo> GetTimeZoneInfosByCountryCode(string countryCode)
        {
            var mapZone = GetMapZonesByCountryCode(countryCode);
            return mapZone
                .Select(x => x.WindowsId)
                .Distinct()
                .Select(GetTimeZoneInfoByTimeZoneId)
                .Distinct();
        }

        /// <summary>
        /// Gets a list of <see cref="MapZone"/> for countryCode
        /// </summary>
        /// <param name="countryCode">2 letter countryCode</param>
        /// <returns></returns>
        private static List<MapZone> GetMapZonesByCountryCode(string countryCode)
        {
            return GetTimeZonesByCountryCode(countryCode)
                .Select(z => z.ZoneId)
                .Distinct()
                .SelectMany(x =>
                    TzdbDateTimeZoneSource.Default.CanonicalIdMap
                        .Where(c => c.Value.Equals(x) || c.Key.Equals(x))
                        .Select(c => c.Key))
                .SelectMany(x => TzdbDateTimeZoneSource.Default.WindowsMapping.MapZones
                    .Where(mz => mz.TzdbIds.Contains(x)))
                .ToList();
        }

        /// <summary>
        /// Gets Available <see cref="TzdbZoneLocation"/> list for countryCode
        /// </summary>
        /// <param name="countryCode">2 letter countryCode</param>
        /// <returns></returns>
        private static List<TzdbZoneLocation> GetTimeZonesByCountryCode(string countryCode)
        {
            return (TzdbDateTimeZoneSource.Default.ZoneLocations ?? throw new InvalidOperationException("ZoneLocations not initialized"))
                .Where(x => x.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private static TimeZoneInfo GetDutchTimeZoneInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetTimeZoneInfoByTimeZoneId("Europe/Amsterdam");
            }
            return GetTimeZoneInfoByCountryCode("NL");
        }

        private static T ValidateSingleResultForCountryCode<T>(this IReadOnlyCollection<T> values, string countryCode)
        {
            if (values.Count > 0)
            {
                if (values.Count == 1)
                {
                    return values.Single();
                }

                throw new ArgumentException($"Multiple results in collection for countryCode '{countryCode}'", nameof(countryCode));
            }

            throw new ArgumentException($"No results in collection for countryCode '{countryCode}'", nameof(countryCode));
        }
    }
}
