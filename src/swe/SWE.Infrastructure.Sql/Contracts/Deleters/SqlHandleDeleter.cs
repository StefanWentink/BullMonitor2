using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlHandleDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlHandleDeleter(
            IFactory<TContext> contextFactory,
            IDeletedHandler<T> handler)
            : base(
                  contextFactory,
                  new StubDeleteValidator<IEnumerable<T>>(),
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}