using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Core.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.TickerValue.Core.Extensions
{
    public static class BullMonitorTickerValueCoreCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerValueCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorTickerValueMapperServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorTickerValueMapperServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddSingleton<
                    IMapper<
                        (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
                        IEnumerable<TipRanksEntity>>,
                    TipRanksResponseMapper>()
                ;
        }
    }
}