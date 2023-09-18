using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateDeleter<TContext, T>
        : SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateDeleter(
            IFactory<TContext> contextFactory,
            IDeleteValidator<T> validator)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  new StubDeletedHandler<IEnumerable<T>>())
        { }
    }
}