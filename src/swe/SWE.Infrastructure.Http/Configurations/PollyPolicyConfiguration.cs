namespace SWE.Infrastructure.Http.Configurations
{
    [Obsolete("Use PolicyConfiguration instead.")]
    public class PollyPolicyConfiguration
    {
        public int CircuitBreakerRetryAttempts { get; set; }
        public int CircuitBreakerDelayMilliseconds { get; set; }
        public int AwaitAndRetryRetryAttempts { get; set; }
        public int AwaitAndRetryDelayMilliseconds { get; set; }
    }
}