using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWE.Issue.Abstractions.Messages;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Tests.Stubs;

namespace SWE.Infrastructure.Web.Extensions
{
    public static class HttpClientConfigurationExtensions
    {
        public static IServiceCollection ConfigureHttpClient(
            this IServiceCollection services,
            IConfiguration configuration,
            string name,
            string? policySectionName = null,
            bool configurePollyPolicy = true)
        {
            return services
                .ConfigureHttpClient(
                    configuration,
                    new StubLogger(),
                    name,
                    policySectionName,
                    configurePollyPolicy);
        }

        public static IServiceCollection ConfigureHttpClient(
            this IServiceCollection services,
                IConfiguration configuration,
                ILogger logger,
                string name,
                string? policySectionName = null,
                bool configurePollyPolicy = true)
        {
            var issueMessageSender = services
                .BuildServiceProvider()
                .GetRequiredService<IMessageSender<IssueMessage>>();

            var httpClientBuilder = services
                .AddHttpClient(name);

            if (configurePollyPolicy)
            {
                httpClientBuilder
                    .AddPolicyHandlers(
                        policySectionName,
                        issueMessageSender,
                        logger,
                        configuration);
            }

            return services;
        }
    }
}