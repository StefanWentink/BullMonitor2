namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ISingleProvider<T>
    {
        Task<T> GetSingleOrDefault(
            CancellationToken cancellationToken);
    }

    public interface ISingleProvider<in TIn, TOut>
    {
        Task<TOut?> GetSingleOrDefault(
            TIn value,
            CancellationToken cancellationToken);
    }
}
