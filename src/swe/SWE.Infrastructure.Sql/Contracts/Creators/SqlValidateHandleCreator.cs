using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleCreator(
            IFactory<TContext> contextFactory,
            ICreateValidator<T> validator,
            ICreatedHandler<T> handler)
            :base(
                contextFactory,
                new CollectionToSingleValidator<T>(validator),
                new CollectionToSingleHandler<T>(handler))
        { }
    }
}