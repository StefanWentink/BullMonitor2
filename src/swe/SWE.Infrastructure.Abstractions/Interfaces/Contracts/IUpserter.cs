namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionAndSingleUpserter<T>
        : IUpserter<T>
        , IUpserter<IEnumerable<T>>
        , ICollectionAndSingleUpserter<T, T>
    { }

    public interface IUpserter<T>
        : IUpserter<T, T>
    { }

    public interface ICollectionAndSingleUpserter<TIn, TOut>
        : IUpserter<TIn, TOut>
        , IUpserter<IEnumerable<TIn>, IEnumerable<TOut>>
    { }

    public interface IUpserter<TIn, TOut>
    {
        Task<TOut> Upsert(TIn value, CancellationToken cancellationToken);
    }
}