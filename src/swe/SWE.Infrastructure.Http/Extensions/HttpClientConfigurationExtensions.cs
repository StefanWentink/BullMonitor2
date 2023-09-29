using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWE.Issue.Abstraction.Messages;
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
            var exceptionMessageSender = services
                .BuildServiceProvider()
                .GetRequiredService<IMessageSender<ExceptionMessage>>();

            var httpClientBuilder = services
                .AddHttpClient(name);

            if (configurePollyPolicy)
            {
                httpClientBuilder
                    .AddPolicyHandlers(
                        policySectionName,
                        exceptionMessageSender,
                        logger,
                        configuration);
            }

            return services;
        }
    }
}