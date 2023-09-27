using BullMonitor.Ticker.Api.SDK.Interfaces;
using BullMonitor.Ticker.Api.SDK.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Web.Extensions;

namespace BullMonitor.Ticker.Api.SDK.Extensions
{
    public static class BullMonitorTickerApiSdkServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerApiSdkServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            return services
                .AddSingleton<IHttpCompanyProvider, HttpCompanyProvider>()
                .ConfigureHttpClient(configuration, "Company")
            ;
        }
    }
}