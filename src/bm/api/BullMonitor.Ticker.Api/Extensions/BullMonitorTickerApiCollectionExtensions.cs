using BullMonitor.Data.Sql.Extensions;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.Mappers;
using BullMonitor.Ticker.Api.Providers;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

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

                .WithBullMonitorCompanyProviderServices(configuration)

                .WithBullMonitorSqlServices(configuration)
                ;
        }
        internal static IServiceCollection WithBullMonitorCompanyProviderServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<ICompanyProvider, CompanyProvider>()
                .AddSingleton<IMapper<TickerEntity, CompanyListResponse>, CompanyListResponseMapper>()
                ;
        }
    }
}
