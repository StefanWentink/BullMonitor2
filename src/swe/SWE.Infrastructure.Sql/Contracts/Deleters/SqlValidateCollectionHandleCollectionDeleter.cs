using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateCollectionHandleCollectionDeleter<TContext, T>
        : SqlBaseDeleter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        protected IDeletedHandler<IEnumerable<T>> Handler { get; }

        protected IValidator<IEnumerable<T>> Validator { get; }

        public SqlValidateCollectionHandleCollectionDeleter(
            IFactory<TContext> contextFactory,
            IDeleteValidator<IEnumerable<T>> validator,
            IDeletedHandler<IEnumerable<T>> handler)
            : base(contextFactory)
        {
            Handler = handler;
            Validator = validator;
        }

        protected override async Task<IEnumerable<T>> IsValid(
            IEnumerable<T> value,
            CancellationToken cancellationToken)
        {
            var response = value.ToList();

            var validationResult = await Validator
                .ValidateAsync(response, cancellationToken)
                .ConfigureAwait(false);

            validationResult
                .GuardValid(GetType().Name);

            return response;
        }

        protected override async Task<IEnumerable<T>> Handle(
            IEnumerable<T> value,
            CancellationToken cancellationToken)
        {
            await Handler
                .Handle(value, cancellationToken)
                .ConfigureAwait(false);

            return value;
        }
    }
}