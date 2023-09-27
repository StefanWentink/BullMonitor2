using SWE.Infrastructure.Abstractions.Interfaces.Requests;

namespace SWE.Infrastructure.Abstractions.Requests
{
    public class ClientRequest
        : IClientRequest
    {
        public ClientRequest(
            TimeZoneInfo requestedTimeZoneInfo)
        {
            RequestedTimeZoneInfo = requestedTimeZoneInfo;
        }

        public TimeZoneInfo RequestedTimeZoneInfo { get; set; }
    }
}