using BullMonitor.Abstractions.Commands;
using BullMonitor.Ticker.Process.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Rabbit.Extensions;

namespace BullMonitor.Ticker.Process.Extensions
{
    public static class BullMonitorTickerProcessServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerProcessServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithBullMonitorTickerProcessorHandlerServices(configuration)
                .WithBullMonitorZacksSyncHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerProcessorHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IMessageHandler<CurrencySyncMessage>, CurrencySyncMessageHandler>()
                .AddSingleton<IMessageHandler<ExchangeSyncMessage>, ExchangeSyncMessageHandler>()
                .AddSingleton<IMessageHandler<TickerSyncMessage>, TickerSyncMessageHandler>()

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