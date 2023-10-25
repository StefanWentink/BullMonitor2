using BullMonitor.Ticker.Core.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;

namespace BullMonitor.Ticker.Api.Extensions
{
    public static class BullMonitorTickerApiCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IDateTimeOffsetNow, DateTimeOffsetNow>()
                .WithBullMonitorTickerCoreServices(configuration)
                ;
        }
    }
}
