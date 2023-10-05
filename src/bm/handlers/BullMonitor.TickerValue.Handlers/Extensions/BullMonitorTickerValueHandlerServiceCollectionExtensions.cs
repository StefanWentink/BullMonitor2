using BullMonitor.Abstractions.Commands;
using BullMonitor.TickerValue.Process.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Configuration.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Process.Interfaces;
using SWE.Process.Models;
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
                .WithRabbitMqSender<IssueModel>()

                //.WithRabbitMqSender<ZacksSyncMessage>()
                .WithRabbitMqReceiver<ZacksSyncMessage, ZacksSyncMessageHandler>(configuration)
                .AddHostedService<RabbitService<ZacksSyncMessage>>()

                //.WithRabbitMqReceiver<ZacksTickerSyncMessage, ZacksTickerSyncMessageHandler>(configuration)
                //.AddHostedService<RabbitService<ZacksTickerSyncMessage>>()

                //.WithRabbitMqSender<TipRanksSyncMessage>()
                //.WithRabbitMqReceiver<TipRanksSyncMessage, TipRanksSyncMessageHandler>(configuration)
                //.AddHostedService<RabbitService<TipRanksSyncMessage>>()
                ;
        }
    }
}