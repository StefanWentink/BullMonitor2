using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Interfaces.Handlers;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateCollectionHandleUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateCollectionHandleUpdater(
            IFactory<TContext> contextFactory,
            IUpdateValidator<IEnumerable<T>> validator,
            IUpdatedHandler<T> handler)
            : base(
                  contextFactory,
                  validator,
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}