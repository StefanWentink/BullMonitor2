using BullMonitor.Abstractions.Commands;
using BullMonitor.Analysis.Process.Handlers;
using SWE.Configuration.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Interfaces;
using SWE.Process.Configurations;
using SWE.Rabbit.Extensions;
using SWE.Rabbit.Receiver;

namespace BullMonitor.Analysis.Handlers.Extensions
{
    public static class BullMonitorAnalysisHandlerServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorAnalysisHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IDateTimeOffsetNow, DateTimeOffsetNow>()
                .WithInfrastructureRabbitMqServices(configuration)
                .AddSingletonConfiguration<ICronProcessConfiguration, CronProcessConfiguration>(configuration)
                .WithRabbitMqSender<IssueMessage>()

                .WithBullMonitorAnalyzeMessageHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorAnalyzeMessageHandlerServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithRabbitMqReceiver<AnalyzeMessage, AnalyzeMessageHandler>(configuration)
                .AddHostedService<RabbitService<AnalyzeMessage>>()
                ;
        }
    }
}