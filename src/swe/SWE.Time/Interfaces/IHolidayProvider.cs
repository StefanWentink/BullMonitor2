using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Time.Interfaces
{
    public interface IHolidayProvider
        : ICollectionProvider<(int year, TimeZoneInfo timeZoneInfo), DateTimeOffset>
        , ICollectionProvider<int, DateTimeOffset>
        , ISingleProvider<DateTimeOffset, bool>
    { }
}