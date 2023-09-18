using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.RabbitMq.Contracts;

namespace SWE.Rabbit.Receiver
{
    public static class RabbitMqReceiverConfigurationExtensions
    {
        public static IServiceCollection WithRabbitMqReceiver<T, THandler>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
            where THandler : class, IMessageHandler<T>
        {
            return serviceCollection
                .AddSingleton<IMessageReceiver<T>, RabbitReceiver<T>>()
                .AddSingleton<IMessageHandler<T>, THandler>()
                ;
        }
    }
}