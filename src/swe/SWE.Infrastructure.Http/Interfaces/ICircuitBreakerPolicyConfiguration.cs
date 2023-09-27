namespace SWE.Infrastructure.Http.Interfaces
{
    public interface ICircuitBreakerPolicyConfiguration
    {
        int RetryCount { get; set; }
        int BreakDurationInSeconds { get; set; }
    }
}