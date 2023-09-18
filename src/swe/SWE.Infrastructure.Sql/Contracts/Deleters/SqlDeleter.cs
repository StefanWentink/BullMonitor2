using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlDeleter(
            IFactory<TContext> contextFactory)
            : base(
                  contextFactory,
                  new StubDeleteValidator<IEnumerable<T>>(),
                  new StubDeletedHandler<IEnumerable<T>>())
        { }
    }
}