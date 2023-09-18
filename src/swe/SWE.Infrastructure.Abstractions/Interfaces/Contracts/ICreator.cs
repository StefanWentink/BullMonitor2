namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionAndSingleCreator<T>
        : ICreator<T>
        , ICreator<IEnumerable<T>>
        , ICollectionAndSingleCreator<T, T>
    { }

    public interface ICreator<T>
        : ICreator<T, T>
    { }

    public interface ICollectionAndSingleCreator<TIn, TOut>
        : ICreator<TIn, TOut>
        , ICreator<IEnumerable<TIn>, IEnumerable<TOut>>
    { }

    public interface ICreator<TIn, TOut>
    {
        Task<TOut> Create(TIn value, CancellationToken cancellationToken);
    }
}