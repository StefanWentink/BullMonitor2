using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Contracts;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class SweSqlServiceCollectionUpdaterExtensions
    {
        public static IServiceCollection WithSqlUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlHandleUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlHandleUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlValidateUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlValidateUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlValidateHandleUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlValidateHandleUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlValidateCollectionHandleUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlValidateHandleCollectionUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlValidateHandleCollectionUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleCollectionUpdater<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, SqlValidateCollectionHandleCollectionUpdater<TContext, T>>()
                .WithSqlBaseUpdater<T>();
        }

        private static IServiceCollection WithSqlBaseUpdater<T>(
            this IServiceCollection serviceCollection)
            where T : class
        {
            return serviceCollection
                .AddSingleton<IUpdater<T>>(x => x.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<T, T>>(x => x.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<IEnumerable<T>, IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                ;
        }
    }
}