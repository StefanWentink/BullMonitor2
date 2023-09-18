using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;
using SWE.Infrastructure.Sql.Stubs;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateCreator<TContext, T>
        : SqlValidateCollectionHandleCollectionCreator<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateCreator(
            IFactory<TContext> contextFactory,
            ICreateValidator<T> validator)
            : base(
                  contextFactory,
                  new CollectionToSingleValidator<T>(validator),
                  new StubCreatedHandler<IEnumerable<T>>())
        { }
    }
}