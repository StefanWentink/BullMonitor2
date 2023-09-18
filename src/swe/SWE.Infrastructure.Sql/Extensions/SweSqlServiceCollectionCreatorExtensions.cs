using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Contracts;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class SweSqlServiceCollectionCreatorExtensions
    {
        public static IServiceCollection WithSqlCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlHandleCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlHandleCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlValidateCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlValidateCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlValidateHandleCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlValidateHandleCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlValidateCollectionHandleCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlValidateHandleCollectionCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlValidateHandleCollectionCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleCollectionCreator<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, SqlValidateCollectionHandleCollectionCreator<TContext, T>>()
                .WithSqlBaseCreator<T>();
        }

        private static IServiceCollection WithSqlBaseCreator<T>(
            this IServiceCollection serviceCollection)
            where T : class
        {
            return serviceCollection
                .AddSingleton<ICreator<T>>(x => x.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<T, T>>(x => x.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<IEnumerable<T>, IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleCreator<T>>())
                ;
        }
    }
}