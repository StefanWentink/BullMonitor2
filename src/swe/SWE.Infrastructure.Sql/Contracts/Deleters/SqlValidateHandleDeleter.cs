using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleDeleter(
            IFactory<TContext> contextFactory,
            IDeleteValidator<T> validator,
            IDeletedHandler<T> handler)
            :base(
                contextFactory,
                new CollectionToSingleValidator<T>(validator),
                new CollectionToSingleHandler<T>(handler))
        { }
    }
}