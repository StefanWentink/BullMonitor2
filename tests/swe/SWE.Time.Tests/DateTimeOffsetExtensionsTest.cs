using FluentAssertions;
using SWE.Time.Extensions;
using SWE.Time.Utilities;
using Xunit;

namespace SWE.Time.Tests
{
    public class DateTimeOffsetExtensionsTest
    {
        [Theory]
        [InlineData(2020, 1)]
        [InlineData(2021, 53)]
        [InlineData(2022, 1)]
        [InlineData(2023, 1)]
        [InlineData(2024, 1)]
        public void GetDutchWeekNumber_Should_Return_WeekNumber(
            int year,
            int expected)
        {
            var referenceDate = new DateTimeOffset(year, 1, 2, 21, 0, 0, TimeSpan.FromHours(-2)); // January 3th

            var actual = referenceDate.GetDutchWeekNumber();

            actual.Should().Be(expected);
        }
        [Theory]
        [InlineData(2020, 4)] // Friday
        [InlineData(2021, 6)] // Sunday
        [InlineData(2022, 0)] // Monday
        [InlineData(2023, 1)] // Tuesday
        [InlineData(2024, 2)] // Wednesday
        public void GetWeekRange_Should_Return_Range(
            int year,
            int daysBack)
        {
            var referenceDate = new DateTimeOffset(year, 1, 2, 21, 0, 0, TimeSpan.FromHours(-2)); // January 3th

            var dutchReferenceDate = referenceDate
                .SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo);

            var expectedFrom = dutchReferenceDate
                .Add(days: -daysBack, timeZoneInfo: TimeZoneInfoUtilities.DutchTimeZoneInfo);

            var expectedUntil = expectedFrom
                .Add(days: 6, timeZoneInfo: TimeZoneInfoUtilities.DutchTimeZoneInfo);

            var actual = referenceDate.GetWeekRange();

            actual.From.Should().Be(expectedFrom);
            actual.Until.Should().Be(expectedUntil);
        }
    }
}