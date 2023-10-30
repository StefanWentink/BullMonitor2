using BullMonitor.Abstractions.Commands;
using BullMonitor.TickerValue.Core.Extensions;
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
                .WithBullMonitorTickerValueCoreServices(configuration)
                .WithBullMonitorTickerValueZacksSyncHandlerServices(configuration)
                .WithBullMonitorTickerValueTipRanksSyncHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerValueZacksSyncHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IMessageHandler<ZacksSyncMessage>, ZacksSyncMessageHandler>()
                .WithRabbitMqSender<ZacksTickerSyncMessage>()
                ;
        }
        internal static IServiceCollection WithBullMonitorTickerValueTipRanksSyncHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IMessageHandler<TipRanksSyncMessage>, TipRanksSyncMessageHandler>()
                .WithRabbitMqSender<TipRanksTickerSyncMessage>()
                ;
        }
    }
}