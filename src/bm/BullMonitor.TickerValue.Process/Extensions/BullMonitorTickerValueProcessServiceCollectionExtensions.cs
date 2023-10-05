using BullMonitor.Abstractions.Commands;
using BullMonitor.TickerValue.Process.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Rabbit.Extensions;

namespace BullMonitor.TickerValue.Process.Extensions
{
    public static class BullMonitorTickerValueProcessServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerValueProcessServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithBullMonitorTickerValueProcessorHandlerServices(configuration)
                .WithBullMonitorZacksSyncHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerValueProcessorHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IMessageHandler<ZacksSyncMessage>, ZacksSyncMessageHandler>()
                ; 
        }

        internal static IServiceCollection WithBullMonitorZacksSyncHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithRabbitMqSender<ZacksTickerSyncMessage>()
                ;
        }
    }
}