using BullMonitor.Abstractions.Commands;
using BullMonitor.Analysis.Core.Extensions;
using BullMonitor.Analysis.Process.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Rabbit.Extensions;

namespace BullMonitor.Analysis.Process.Extensions
{
    public static class BullMonitorTickerValueProcessServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorAnalysisProcessServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithBullMonitorAnalysisCoreServices(configuration)
                .WithBullMonitorAnalysisSyncHandlerServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorAnalysisSyncHandlerServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<IMessageHandler<AnalyzeMessage>, AnalyzeMessageHandler>()
                .WithRabbitMqSender<AnalyzeMessage>()
                ;
        }
    }
}