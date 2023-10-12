using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Mongo.Interfaces
{
    public interface IBaseMongoConnector<T>
        : IBaseMongoConnector<IMongoConditionContainer<T>, T>
    { }

    public interface IBaseMongoConnector<TContainer, T>
        : IProvider<TContainer, T>
        , ICollectionAndSingleUpserter<T>
        , ICollectionAndSingleUpdater<T>
        , ICollectionAndSingleCreator<T>
        where TContainer : IMongoConditionContainer<T>
    { }

    public interface IBaseMongoConnector<TContainer, T, TProjection>
        : IProvider<TContainer, TProjection>
        , ICollectionAndSingleUpserter<T>
        , ICollectionAndSingleUpdater<T>
        , ICollectionAndSingleCreator<T>
        where TContainer : IMongoConditionContainer<T, TProjection>
    { }
}