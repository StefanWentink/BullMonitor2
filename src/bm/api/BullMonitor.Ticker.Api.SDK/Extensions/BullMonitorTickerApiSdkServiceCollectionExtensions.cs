using BullMonitor.Ticker.Api.SDK.Interfaces;
using BullMonitor.Ticker.Api.SDK.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Configuration.Extensions;
using SWE.Infrastructure.Http.Configurations;
using SWE.Infrastructure.Http.Interfaces;
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
                .AddSingletonConfiguration<IHttpClientConfiguration, HttpClientConfiguration>(configuration, $"{nameof(HttpClientConfiguration)}:Company")
                .ConfigureHttpClient(configuration, "Company")
                .AddSingleton<IHttpCompanyProvider, HttpCompanyProvider>()
            ;
        }
    }
}