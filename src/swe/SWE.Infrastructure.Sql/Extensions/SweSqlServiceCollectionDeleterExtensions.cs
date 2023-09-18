using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Contracts;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class SweSqlServiceCollectionDeleterExtensions
    {
        public static IServiceCollection WithSqlDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlHandleDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlHandleDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlValidateDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlValidateDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlValidateHandleDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlValidateHandleDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlValidateCollectionHandleDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlValidateHandleCollectionDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlValidateHandleCollectionDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        public static IServiceCollection WithSqlValidateCollectionHandleCollectionDeleter<T, TContext>(
            this IServiceCollection serviceCollection)
            where T : class
            where TContext : DbContext
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, SqlValidateCollectionHandleCollectionDeleter<TContext, T>>()
                .WithSqlBaseDeleter<T>();
        }

        private static IServiceCollection WithSqlBaseDeleter<T>(
            this IServiceCollection serviceCollection)
            where T : class
        {
            return serviceCollection
                .AddSingleton<IDeleter<T>>(x => x.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<T, T>>(x => x.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<IEnumerable<T>, IEnumerable<T>>>(x => x.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                ;
        }
    }
}