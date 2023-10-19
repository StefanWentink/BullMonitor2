using Microsoft.Extensions.DependencyInjection;
using SWE.Time.Providers;
using Microsoft.Extensions.Configuration;
using SWE.Time.Interfaces;

namespace SWE.Time.Extensions
{
    public static class SweTimeServiceCollectionExtensions
    {
        public static IServiceCollection WithSweTimeServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithHolidayProviderService()
                ;
        }
        public static IServiceCollection WithHolidayProviderService(
            this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IHolidayProvider, HolidayProvider >()
                ;
        }
    }
}