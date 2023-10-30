using BullMonitor.Abstractions.Commands;
using BullMonitor.TickerValue.Process.Handlers;
using SWE.Configuration.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Interfaces;
using SWE.Process.Configurations;
using SWE.Rabbit.Extensions;
using SWE.Rabbit.Receiver;

namespace BullMonitor.TickerValue.Handlers.Extensions
{
    public static class BullMonitorTickerValueHandlerServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerValueHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IDateTimeOffsetNow, DateTimeOffsetNow>()
                .WithInfrastructureRabbitMqServices(configuration)
                .AddSingletonConfiguration<ICronProcessConfiguration, CronProcessConfiguration>(configuration)
                .WithRabbitMqSender<IssueMessage>()

                .WithBullMonitorTickerValueZacksHandlerServices(configuration)

                .WithBullMonitorTickerValueTipRanksHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerValueZacksHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithRabbitMqReceiver<ZacksSyncMessage, ZacksSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<ZacksSyncMessage>>()

                .WithRabbitMqReceiver<ZacksTickerSyncMessage, ZacksTickerSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<ZacksTickerSyncMessage>>()
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerValueTipRanksHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithRabbitMqReceiver<TipRanksSyncMessage, TipRanksSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<TipRanksSyncMessage>>()

                .WithRabbitMqReceiver<TipRanksTickerSyncMessage, TipRanksTickerSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<TipRanksTickerSyncMessage>>()
                ;
        }
    }
}