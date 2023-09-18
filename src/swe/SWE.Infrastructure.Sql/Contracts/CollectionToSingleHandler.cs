using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class CollectionToSingleHandler<T>
        : IHandler<IEnumerable<T>>
        , ICreatedHandler<IEnumerable<T>>
        , IUpdatedHandler<IEnumerable<T>>
        , IDeletedHandler<IEnumerable<T>>
    {
        protected IHandler<T> Handler { get; }

        public CollectionToSingleHandler(IHandler<T> handler)
        {
            Handler = handler;
        }

        public async Task Handle(IEnumerable<T> value, CancellationToken cancellationToken)
        {
            var tasks = value.Select(x => Handler.Handle(x, cancellationToken));

            await Task
                .WhenAll(tasks)
                .ConfigureAwait(false);
        }
    }
}