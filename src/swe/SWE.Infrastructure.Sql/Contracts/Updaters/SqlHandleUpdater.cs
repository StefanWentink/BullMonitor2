using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlHandleUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlHandleUpdater(
            IFactory<TContext> contextFactory,
            IUpdatedHandler<T> handler)
            : base(
                  contextFactory,
                  new StubUpdateValidator<IEnumerable<T>>(),
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}