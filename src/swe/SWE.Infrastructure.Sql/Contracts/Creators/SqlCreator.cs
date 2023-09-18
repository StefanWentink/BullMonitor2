using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlCreator(
            IFactory<TContext> contextFactory)
            : base(
                  contextFactory,
                  new StubCreateValidator<IEnumerable<T>>(),
                  new StubCreatedHandler<IEnumerable<T>>())
        { }
    }
}