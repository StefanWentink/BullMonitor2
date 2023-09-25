using Microsoft.Extensions.DependencyInjection;
using SWE.Mongo.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Mongo;

namespace SWE.Mongo.Extensions
{
    public static class SweMongoServiceCollectionExtensions
    {
        public static IServiceCollection WithMongoConnectorServices<T, TConnector>(
            this IServiceCollection serviceCollection)
            where TConnector : class, IBaseMongoConnector<T>
        {
            return serviceCollection
                .AddSingleton<IBaseMongoConnector<T>, TConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<T>, T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleUpserter<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleUpdater<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleCreator<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                ;
        }

        public static IServiceCollection WithMongoConnectorServices<TContainer, T, TProjection, TConnector>(
            this IServiceCollection serviceCollection)
            where TContainer : IMongoConditionContainer<T, TProjection>
            where TConnector : BaseMongoConnector<TContainer, T, TProjection>
        {
            return serviceCollection
                .AddSingleton<IBaseMongoConnector<TContainer, T, TProjection>, TConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<T>, T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleUpserter<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleUpdater<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                .AddSingleton<ICollectionAndSingleCreator<T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())
                ;
        }
    }
}