using SWE.Infrastructure.Http.Interfaces;

namespace SWE.Infrastructure.Http.Configurations
{
    public class PolicyConfiguration
        : ICircuitBreakerPolicyConfiguration
        , IRetryPolicyConfiguration
    {
        public int RetryCount { get; set; }
        public int BreakDurationInSeconds { get; set; }
        public bool BreakDurationExponential { get; set; } = true;
        public int? ClientRequestTimeoutInSeconds { get; set; }
        public int? MaxTotalRetryDurationInSeconds { get; set; }
    }
}