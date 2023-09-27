namespace SWE.Infrastructure.Http.Interfaces
{
    public interface IRetryPolicyConfiguration
    {
        int RetryCount { get; set; }
        int BreakDurationInSeconds { get; set; }
        bool BreakDurationExponential { get; set; }
        int? ClientRequestTimeoutInSeconds { get; set; }
        int? MaxTotalRetryDurationInSeconds { get; set; }
    }
}