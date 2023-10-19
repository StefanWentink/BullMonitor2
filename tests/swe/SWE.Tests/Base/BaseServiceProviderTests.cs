using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SWE.Tests.Base
{
    public abstract class BaseServiceProviderTests<T>
        : BaseDiTests
        where T : notnull
    {
        protected override IServiceCollection AddBaseServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services = base
                .AddBaseServices(
                    services,
                    configuration);

            return WithServices(
                    services,
                    configuration);
        }

        protected abstract IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration);

        /// <summary>
        /// Uses a pre-build <see cref="ServiceProvider"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected virtual T Create()
        {
            return ServiceProvider
                .GetRequiredService<T>();
        }

        /// <summary>
        /// Will create a <see cref="ServiceProvider"/> on the fly.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected virtual T Create(IServiceCollection services)
        {
            return services
                .BuildServiceProvider()
                .GetRequiredService<T>();
        }
    }
}