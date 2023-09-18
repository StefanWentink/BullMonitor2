using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static  class SweSqlServiceCollectionProviderExtensions
    {
        public static IServiceCollection WithSqlProvider<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .WithSqlProvider<T, TContext, SqlProvider<T, TContext>>()
                ;
        }

        public static IServiceCollection WithSqlProvider<T, TContext, TProvider>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
            where TProvider : class, ISqlProvider<T>
        {
            return serviceCollection
                .AddSingleton<ISqlProvider<T>, TProvider>()
                .AddSingleton<IProvider<ISqlConditionContainer<T>, T>>(
                    serviceCollection => serviceCollection.GetRequiredService<ISqlProvider<T>>())
                //.AddSingleton<IGetter<T>>(
                //    serviceCollection => serviceCollection.GetRequiredService<ISqlProvider<T>>())
                ;
        }
    }
}