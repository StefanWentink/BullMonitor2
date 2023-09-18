using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Interfaces.Handlers;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleCollectionUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleCollectionUpdater(
            IFactory<TContext> contextFactory,
            IUpdateValidator<T> validator,
            IUpdatedHandler<IEnumerable<T>> handler)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  handler)
        { }
    }
}