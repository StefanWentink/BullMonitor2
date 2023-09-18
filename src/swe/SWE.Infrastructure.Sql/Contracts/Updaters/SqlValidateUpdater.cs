using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateUpdater(
            IFactory<TContext> contextFactory,
            IUpdateValidator<T> validator)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  new StubUpdatedHandler<IEnumerable<T>>())
        { }
    }
}