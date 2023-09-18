using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateCollectionHandleCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateCollectionHandleCreator(
            IFactory<TContext> contextFactory,
            ICreateValidator<IEnumerable<T>> validator,
            ICreatedHandler<T> handler)
            : base(
                  contextFactory,
                  validator,
                  new CollectionToSingleHandler<T>(handler))
        { }
    }
}