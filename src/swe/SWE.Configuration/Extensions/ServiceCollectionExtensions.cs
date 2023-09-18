using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SWE.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonConfiguration<TI, T>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
            where T : class, TI, new()
        {
            return serviceCollection
                .AddSingletonConfiguration<TI, T>(
                    configuration,
                    typeof(T).Name);
        }

        public static IServiceCollection AddSingletonConfiguration<TI, T>(
            this IServiceCollection serviceCollection,
            string rootTag,
            IConfiguration configuration)
            where T : class, TI, new()
        {
            var tag = (string.IsNullOrEmpty(rootTag)
                ? typeof(T).Name
                : $"{rootTag}:{typeof(T).Name}");

            return serviceCollection
                .AddSingletonConfiguration<TI, T>(
                    configuration,
                    tag);
        }

        public static IServiceCollection AddSingletonConfiguration<TI, T>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            string section)
            where T : class, TI, new()
        {
            var config = configuration
                .GetSection(section)
                .Get<T>() ?? new T();

            return serviceCollection
                .AddSingleton(config)
                .AddSingleton(typeof(TI), x => x.GetRequiredService<T>());
        }
    }
}