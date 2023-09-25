using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.DataMine.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.DataMine.Extensions
{
    public static class BullMonitorDataMineServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorDataMineServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .WithDataMineTipRanksServices(configuration)
                .WithDataMineZacksRankServices(configuration)
                ;
        }

        internal static IServiceCollection WithDataMineTipRanksServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<ISingleProvider<TipRanksRequest, TipRanksResponse>, TipRanksProvider>()
                .AddSingleton<ISingleProvider<TipRanksRequest, string?>, TipRanksRawProvider>()
                ;
        }

        internal static IServiceCollection WithDataMineZacksRankServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<ISingleProvider<ZacksRankRequest, ZacksRankResponse>, ZacksRankProvider>()
                .AddSingleton<ISingleProvider<ZacksRankRequest, string?>, ZacksRankRawProvider>()
                ;
        }
    }
}