using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlHandleCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlHandleCreator(
            IFactory<TContext> contextFactory,
            ICreatedHandler<T> handler)
            : base(
                  contextFactory,
                  new StubCreateValidator<IEnumerable<T>>(),
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}