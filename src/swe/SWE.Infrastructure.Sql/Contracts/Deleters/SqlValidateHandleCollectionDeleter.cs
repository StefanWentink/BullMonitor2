using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleCollectionDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleCollectionDeleter(
            IFactory<TContext> contextFactory,
            IDeleteValidator<T> validator,
            IDeletedHandler<IEnumerable<T>> handler)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  handler)
        { }
    }
}