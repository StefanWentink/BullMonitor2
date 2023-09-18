using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE.Infrastructure.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithCreator<TCreator, T>(
            this IServiceCollection serviceCollection)
            where TCreator : class, ICollectionAndSingleCreator<T>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<T>, TCreator>()
                .AddSingleton<ICreator<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<T>>(services => services.GetRequiredService<ICollectionAndSingleCreator<T>>())
        
                .AddSingleton<ICollectionAndSingleCreator<T, T>>(services => services.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleCreator<T>>())
                .AddSingleton<ICreator<T, T>>(services => services.GetRequiredService<ICollectionAndSingleCreator<T>>());
        }

        public static IServiceCollection WithCreator<TCreator, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TCreator : class, ICollectionAndSingleCreator<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleCreator<TIn, TOut>, TCreator>()
                .AddSingleton<ICreator<IEnumerable<TIn>, IEnumerable<TOut>>>(services => services.GetRequiredService<ICollectionAndSingleCreator<TIn, TOut>>())
                .AddSingleton<ICreator<TIn, TOut>>(services => services.GetRequiredService<ICollectionAndSingleCreator<TIn, TOut>>());
        }

        public static IServiceCollection WithUpdater<TUpdater, T>(
            this IServiceCollection serviceCollection)
            where TUpdater : class, ICollectionAndSingleUpdater<T>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<T>, TUpdater>()
                .AddSingleton<IUpdater<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<T>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<T>>())

                .AddSingleton<ICollectionAndSingleUpdater<T, T>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<T>>())
                .AddSingleton<IUpdater<T, T>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<T>>());
        }

        public static IServiceCollection WithUpdater<TUpdater, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TUpdater : class, ICollectionAndSingleUpdater<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpdater<TIn, TOut>, TUpdater>()
                .AddSingleton<IUpdater<IEnumerable<TIn>, IEnumerable<TOut>>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<TIn, TOut>>())
                .AddSingleton<IUpdater<TIn, TOut>>(services => services.GetRequiredService<ICollectionAndSingleUpdater<TIn, TOut>>());
        }

        public static IServiceCollection WithUpserter<TUpserter, T>(
            this IServiceCollection serviceCollection)
            where TUpserter : class, ICollectionAndSingleUpserter<T>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpserter<T>, TUpserter>()
                .AddSingleton<IUpserter<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<T>>())
                .AddSingleton<IUpserter<T>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<T>>())

                .AddSingleton<ICollectionAndSingleUpserter<T, T>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<T>>())
                .AddSingleton<IUpserter<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<T>>())
                .AddSingleton<IUpserter<T, T>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<T>>());
        }

        public static IServiceCollection WithUpserter<TUpserter, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TUpserter : class, ICollectionAndSingleUpserter<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleUpserter<TIn, TOut>, TUpserter>()
                .AddSingleton<IUpserter<IEnumerable<TIn>, IEnumerable<TOut>>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<TIn, TOut>>())
                .AddSingleton<IUpserter<TIn, TOut>>(services => services.GetRequiredService<ICollectionAndSingleUpserter<TIn, TOut>>());
        }

        public static IServiceCollection WithDeleter<TDeleter, T>(
            this IServiceCollection serviceCollection)
            where TDeleter : class, ICollectionAndSingleDeleter<T>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<T>, TDeleter>()
                .AddSingleton<IDeleter<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<T>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<T>>())

                .AddSingleton<ICollectionAndSingleDeleter<T, T>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<IEnumerable<T>, IEnumerable<T>>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<T>>())
                .AddSingleton<IDeleter<T, T>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<T>>());
        }

        public static IServiceCollection WithDeleter<TDeleter, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TDeleter : class, ICollectionAndSingleDeleter<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ICollectionAndSingleDeleter<TIn, TOut>, TDeleter>()
                .AddSingleton<IDeleter<IEnumerable<TIn>, IEnumerable<TOut>>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<TIn, TOut>>())
                .AddSingleton<IDeleter<TIn, TOut>>(services => services.GetRequiredService<ICollectionAndSingleDeleter<TIn, TOut>>());
        }

        public static IServiceCollection WithDataProvider<TProvider, TCondition, T>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IDataProvider<TCondition, T>
            where TCondition : IConditionContainer<T>
        {
            return serviceCollection
                .AddSingleton<IDataProvider<TCondition, T>, TProvider>()
                .AddSingleton<IProvider<TCondition, T>>(services => services.GetRequiredService<IDataProvider<TCondition, T>>())
                .AddSingleton<ICollectionProvider<TCondition, T>>(services => services.GetRequiredService<IDataProvider<TCondition, T>>())
                .AddSingleton<ISingleProvider<TCondition, T>>(services => services.GetRequiredService<IDataProvider<TCondition, T>>());
        }

        public static IServiceCollection WithProvider<TProvider, T>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<T>
        {
            return serviceCollection
                .AddSingleton<IProvider<T>, TProvider>()
                .AddSingleton<ICollectionProvider<T>>(services => services.GetRequiredService<IProvider<T>>())
                .AddSingleton<ISingleProvider<T>>(services => services.GetRequiredService<IProvider<T>>());
        }

        public static IServiceCollection WithProvider<TProvider, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<IProvider<TIn, TOut>, TProvider>()
                .AddSingleton<ICollectionProvider<TIn, TOut>>(services => services.GetRequiredService<IProvider<TIn, TOut>>())
                .AddSingleton<ISingleProvider<TIn, TOut>>(services => services.GetRequiredService<IProvider<TIn, TOut>>());
        }

        internal static IServiceCollection WithCollectionProvider<TProvider, T>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<T>
        {
            return serviceCollection
                .AddSingleton<ICollectionProvider<T>, TProvider>();
        }

        internal static IServiceCollection WithSingleProvider<TProvider, T>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<T>
        {
            return serviceCollection
                .AddSingleton<ISingleProvider<T>, TProvider>();
        }

        internal static IServiceCollection WithCollectionProvider<TProvider, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ICollectionProvider<TIn, TOut>, TProvider>();
        }

        internal static IServiceCollection WithSingleProvider<TProvider, TIn, TOut>(
            this IServiceCollection serviceCollection)
            where TProvider : class, IProvider<TIn, TOut>
        {
            return serviceCollection
                .AddSingleton<ISingleProvider<TIn, TOut>, TProvider>();
        }
    }
}