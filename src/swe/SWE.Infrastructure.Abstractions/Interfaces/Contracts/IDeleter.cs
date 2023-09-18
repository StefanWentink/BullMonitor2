namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionAndSingleDeleter<T>
        : IDeleter<T>
        , IDeleter<IEnumerable<T>>
        , ICollectionAndSingleDeleter<T, T>
    { }

    public interface IDeleter<T>
        : IDeleter<T, T>
    { }

    public interface ICollectionAndSingleDeleter<TIn, TOut>
        : IDeleter<TIn, TOut>
        , IDeleter<IEnumerable<TIn>, IEnumerable<TOut>>
    { }

    public interface IDeleter<TIn, TOut>
    {
        Task<TOut> Delete(TIn value, CancellationToken cancellationToken);
    }
}