using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleCollectionCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleCollectionCreator(
            IFactory<TContext> contextFactory,
            ICreateValidator<T> validator,
            ICreatedHandler<IEnumerable<T>> handler)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  handler)
        { }
    }
}