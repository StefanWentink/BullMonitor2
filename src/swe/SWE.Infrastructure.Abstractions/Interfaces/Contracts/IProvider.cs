namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface IProvider<T>
        : ICollectionProvider<T>
        , ISingleProvider<T>
    { }

    public interface IProvider<in TIn, TOut>
        : ICollectionProvider<TIn, TOut>
        , ISingleProvider<TIn, TOut>
    { }
}