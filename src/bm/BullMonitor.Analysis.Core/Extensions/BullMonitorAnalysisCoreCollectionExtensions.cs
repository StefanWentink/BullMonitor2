using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BullMonitor.Analysis.Core.Extensions
{
    public static class BullMonitorAnalysisCoreCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorAnalysisCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                //.WithBullMonitorTickerValueMapperServices(configuration)
                ;
        }
    }
}