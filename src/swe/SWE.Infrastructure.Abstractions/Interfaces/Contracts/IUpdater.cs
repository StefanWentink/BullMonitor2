namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionAndSingleUpdater<T>
        : IUpdater<T>
        , IUpdater<IEnumerable<T>>
        , ICollectionAndSingleUpdater<T, T>
    { }

    public interface IUpdater<T>
        : IUpdater<T, T>
    { }

    public interface ICollectionAndSingleUpdater<TIn, TOut>
        : IUpdater<TIn, TOut>
        , IUpdater<IEnumerable<TIn>, IEnumerable<TOut>>
    { }

    public interface IUpdater<TIn, TOut>
    {
        Task<TOut> Update(TIn value, CancellationToken cancellationToken);
    }
}