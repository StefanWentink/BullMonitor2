using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using SWE.Issue.Abstraction.Messages;
using SWE.Infrastructure.Http.Configurations;
using SWE.Infrastructure.Web.Policies;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Infrastructure.Web.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddPolicyHandlers(
            this IHttpClientBuilder httpClientBuilder,
            string? policySectionName,
            IMessageSender<ExceptionMessage> exceptionMessageSender,
            ILogger logger,
            IConfiguration configuration)
        {
            var cancellationToken = new CancellationToken();
            var policyConfiguration = new PolicyConfiguration();

            configuration.Bind(policySectionName ?? nameof(PolicyConfiguration), policyConfiguration);

            var policyWrap = Policy
                .WrapAsync(
                    HttpCircuitBreakerPolicies.GetHttpCircuitBreakerPolicy(exceptionMessageSender, logger, policyConfiguration, cancellationToken),
                    HttpRetryPolicies.GetHttpRetryPolicy(exceptionMessageSender, logger, policyConfiguration, cancellationToken));

            return httpClientBuilder.AddPolicyHandler(policyWrap);
        }

        public static IHttpClientBuilder AddClientConfiguration(
             this IHttpClientBuilder httpClientBuilder,
             string policySectionName,
             IConfiguration configuration)
        {
            var policyConfig = new PolicyConfiguration();
            configuration
                .Bind(policySectionName, policyConfig);

            httpClientBuilder
                .ConfigureHttpClient((client) => client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(policyConfig.ClientRequestTimeoutInSeconds ?? 100))); // 100s is default timeout 

            return httpClientBuilder;
        }
    }
}