namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ICollectionAndSingleMapper<TIn, TOut>
        : IMapper<TIn, TOut>
        , IMapper<IEnumerable<TIn>, IEnumerable<TOut>>
    { }

    public interface IMapper<TIn, TOut>
    {
        Task<TOut> Map(TIn value, CancellationToken cancellationToken);
    }
}