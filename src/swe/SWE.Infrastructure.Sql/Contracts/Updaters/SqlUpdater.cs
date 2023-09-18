using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Stubs;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlUpdater(
            IFactory<TContext> contextFactory)
            : base(
                  contextFactory,
                  new StubUpdateValidator<IEnumerable<T>>(),
                  new StubUpdatedHandler<IEnumerable<T>>())
        { }
    }
}