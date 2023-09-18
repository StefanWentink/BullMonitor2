using BullMonitor.Data.Sql.Contexts;
using BullMonitor.Data.Sql.Factories;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Extensions;

namespace BullMonitor.Data.Sql.Extensions
{
    public static class BullMonitorSqlServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorSqlServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .AddDbContextWithDefaultOptions<TickerContext>(configuration, "Ticker")
                .AddSingleton<IFactory<TickerContext>, TickerContextFactory>()

                .WithBullMonitorSqlServices<CurrencyEntity>(configuration)
                .WithBullMonitorSqlServices<ExchangeEntity>(configuration)
                .WithBullMonitorSqlServices<IndustryEntity>(configuration)
                .WithBullMonitorSqlServices<SectorEntity>(configuration)
                .WithBullMonitorSqlServices<TickerEntity>(configuration);
        }

        internal static IServiceCollection WithBullMonitorSqlServices<T>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
            where T : class
        {
            return serviceCollection
                .WithSqlProvider<T, TickerContext>()
                .WithSqlCreator<T, TickerContext>()
                .WithSqlUpdater<T, TickerContext>()
                .WithSqlDeleter<T, TickerContext>()
                ;
        }
    }
}