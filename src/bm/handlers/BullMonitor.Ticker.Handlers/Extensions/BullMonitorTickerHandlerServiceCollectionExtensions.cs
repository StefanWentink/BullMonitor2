using BullMonitor.Abstractions.Commands;
using BullMonitor.Ticker.Process.Handlers;
using SWE.Configuration.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Interfaces;
using SWE.Process.Configurations;
using SWE.Rabbit.Extensions;
using SWE.Rabbit.Receiver;

namespace BullMonitor.Ticker.Handlers.Extensions
{
    public static class BullMonitorTickerHandlerServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IDateTimeOffsetNow, DateTimeOffsetNow>()
                .WithInfrastructureRabbitMqServices(configuration)
                .AddSingletonConfiguration<ICronProcessConfiguration, CronProcessConfiguration>(configuration)
                .WithRabbitMqSender<IssueMessage>()

                //.WithRabbitMqSender<ExchangeSyncMessage>()
                .WithRabbitMqReceiver<ExchangeSyncMessage, ExchangeSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<ExchangeSyncMessage>>()

                //.WithRabbitMqSender<CurrencySyncMessage>()
                .WithRabbitMqReceiver<CurrencySyncMessage, CurrencySyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<CurrencySyncMessage>>()

                //.WithRabbitMqSender<TickerSyncMessage>()
                .WithRabbitMqReceiver<TickerSyncMessage, TickerSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<TickerSyncMessage>>()

                //.WithRabbitMqSender<ZacksSyncMessage>()
                //.WithRabbitMqReceiver<ZacksSyncMessage, ZacksSyncMessageHandler>(configuration)
                //.AddHostedService<RabbitService<ZacksSyncMessage>>()

                //.WithRabbitMqSender<TipRanksSyncMessage>()
                //.WithRabbitMqReceiver<TipRanksSyncMessage, TipRanksSyncMessageHandler>(configuration)
                //.AddHostedService<RabbitService<TipRanksSyncMessage>>()
                ;
        }
    }
}