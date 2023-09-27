namespace SWE.Infrastructure.Abstractions.Interfaces.Requests
{
    public interface IClientRequest
    {
        TimeZoneInfo RequestedTimeZoneInfo { get; set; }
    }
}