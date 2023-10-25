using BullMonitor.Data.Sql.Extensions;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Core.Mappers;
using BullMonitor.Ticker.Core.Providers;
using BullMonitor.Ticker.Core.Updaters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Extensions.Interfaces;
using SWE.Extensions.Models;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Core.Extensions
{
    public static class BullMonitorTickerApiCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<IDateTimeOffsetNow, DateTimeOffsetNow>()
                .WithBullMonitorCompanyProviderServices(configuration)
                .WithBullMonitorCompanyUpdaterServices(configuration)
                .WithBullMonitorSqlServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorCompanyUpdaterServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<ICompanyUpdater, CompanyUpdater>()
                .AddSingleton<IMapper<TickerEntity, CompanyListResponse>, CompanyListResponseMapper>()
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