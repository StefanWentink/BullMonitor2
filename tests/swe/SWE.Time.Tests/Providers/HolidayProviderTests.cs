using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Tests.Base;
using SWE.Time.Utilities;
using SWE.Time.Extensions;
using SWE.Time.Interfaces;
using Xunit;

namespace Swe.Time.Tests.Providers
{
    public class HolidayProviderTests
        : BaseServiceProviderTests<IHolidayProvider>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithHolidayProviderService();
        }

        [Theory()]
        [InlineData(1978, 5, 4)]    // Queensday on monday
        [InlineData(1989, 5, 4)]    // Queensday on saturday
        [InlineData(2008, 5, 1)]    // second to first eastern
        [InlineData(2013, 5, 9)]
        [InlineData(2017, 5, 25)]    // Kingsday on saturday
        [InlineData(2023, 5, 18)]
        [InlineData(2033, 5, 26)]
        [InlineData(2038, 6, 3)]    // latest eastern
        public async Task HolidayProvider_Should_GetEastern(
            int year,
            int month,
            int day)
        {
            var timeZoneInfo = TimeZoneInfoUtilities.DutchTimeZoneInfo;

            var expected = new DateTime(year, month, day).ToDateTimeOffset(timeZoneInfo);
            var expectedKingsDay = year >= 2014
                ? new DateTime(year, 4, 27).ToDateTimeOffset(timeZoneInfo)
                : new DateTime(year, 4, 30).ToDateTimeOffset(timeZoneInfo);

            if (expectedKingsDay.DayOfWeek == DayOfWeek.Sunday)
            {
                expectedKingsDay = year >= 1980
                    ? expectedKingsDay.Add(days: -1, timeZoneInfo: timeZoneInfo)
                    : new DateTime(year, 5, 1).ToDateTimeOffset(timeZoneInfo);
            }

            var expectedNewYear = new DateTime(year, 1, 1).ToDateTimeOffset(timeZoneInfo);
            var expectedChristmasDay = new DateTime(year, 12, 25).ToDateTimeOffset(timeZoneInfo);
            var expectedChristmasDaySecond = new DateTime(year, 12, 26).ToDateTimeOffset(timeZoneInfo);

            var provider = Create();

            var holidays = await provider
                .Get((year, timeZoneInfo), CancellationToken.None)
                .ConfigureAwait(false);

            holidays.Should().Contain(expected);
            holidays.Should().Contain(expectedKingsDay);
            holidays.Should().Contain(expectedNewYear);
            holidays.Should().Contain(expectedChristmasDay);
            holidays.Should().Contain(expectedChristmasDaySecond);
        }
    }
}