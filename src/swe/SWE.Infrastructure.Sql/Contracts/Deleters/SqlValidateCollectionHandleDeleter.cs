using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateCollectionHandleDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateCollectionHandleDeleter(
            IFactory<TContext> contextFactory,
            IDeleteValidator<IEnumerable<T>> validator,
            IDeletedHandler<T> handler)
            : base(
                  contextFactory,
                  validator,
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}