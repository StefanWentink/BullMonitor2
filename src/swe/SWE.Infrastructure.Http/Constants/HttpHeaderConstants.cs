namespace SWE.Infrastructure.Http.Constants
{
    public static class HttpHeaderConstants
    {
        public const string TimeZone = nameof(TimeZone);
        public const string Authorization = nameof(Authorization);
        public const string Basic = nameof(Basic);
        public const string TotalCount = "X-Total-Count";
        public const string ResponseTimeInMs = "X-Response-Time-Milliseconds";
    }
}