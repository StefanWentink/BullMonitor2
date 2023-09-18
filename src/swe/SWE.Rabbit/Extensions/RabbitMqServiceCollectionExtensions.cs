using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.RabbitMq.Interfaces;
using SWE.RabbitMq.Contracts;
using SWE.Infrastructure.Abstractions.Models;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Configuration.Extensions;
using SWE.Rabbit.Abstractions.Messages;
using SWE.Rabbit.Configuration;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Rabbit.Extensions
{
    public static class RabbitMqConfigurationExtensions
    {
        public static IServiceCollection WithInfrastructureRabbitMqServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithInfrastructureRabbitMqConfigurationServices(configuration)
                ;
        }

        internal static IServiceCollection WithInfrastructureRabbitMqConfigurationServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection = serviceCollection
                .AddSingletonConfiguration<IRabbitConfiguration, RabbitConfiguration>(configuration)
                ;

            var rabbitConfiguration = serviceCollection
                .BuildServiceProvider()
                .GetService<IRabbitConfiguration>();

            if (rabbitConfiguration != null)
            {
                var IssueModelQueues = Enum
                    .GetValues<LogLevel>()
                    .Select(logLevel => new RabbitQueueConfiguration(
                        $"{nameof(IssueModel)}_{logLevel}",
                        logLevel.ToString()));

                var IssueModelExchangeConfiguration = new RabbitExchangeConfiguration(
                    nameof(IssueModel),
                    IssueModelQueues);

                rabbitConfiguration.AddExchange(IssueModelExchangeConfiguration);

                serviceCollection = serviceCollection
                    .AddSingleton(rabbitConfiguration)
                    ;
            }

            return serviceCollection;
        }

        public static IServiceCollection WithRabbitMqSender<T>(
            this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IMessageSender<T>, RabbitSender<T>>()
                .AddSingleton<ISender<T>>(x => x.GetRequiredService<IMessageSender<T>>())
                .AddSingleton<ISender<RoutingMessage<T>>>(x => x.GetRequiredService<IMessageSender<T>>())
                ;
        }
    }
}