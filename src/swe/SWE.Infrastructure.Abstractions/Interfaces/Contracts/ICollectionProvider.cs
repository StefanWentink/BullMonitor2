namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionProvider<T>
    {
        Task<IEnumerable<T>> Get(
            CancellationToken cancellationToken);
    }

    public interface ICollectionProvider<in TIn, TOut>
    {
        Task<IEnumerable<TOut>> Get(
            TIn value,
            CancellationToken cancellationToken);
    }
}
