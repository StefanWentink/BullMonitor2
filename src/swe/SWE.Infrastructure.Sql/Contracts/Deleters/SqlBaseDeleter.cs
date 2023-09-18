using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Exceptions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Extensions;

namespace SWE.Infrastructure.Sql.Contracts
{
    public abstract class SqlBaseDeleter<TContext, T>
        : ICollectionAndSingleDeleter<T>
        where TContext : DbContext
        where T : class
    {
        protected IFactory<TContext> ContextFactory { get; }

        public SqlBaseDeleter(
            IFactory<TContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public virtual async Task<T> Delete(
            T value,
            CancellationToken cancellationToken = default)
        {
            var response = await Delete(
                    new HashSet<T> { value },
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Single();
        }

        public virtual async Task<IEnumerable<T>> Delete(
            IEnumerable<T> value,
            CancellationToken cancellationToken = default)
        {
            var values = await IsValid(value, cancellationToken)
                .ConfigureAwait(false);

            var response = await Execute(values, cancellationToken)
                .ConfigureAwait(false);

            var responses = await Handle(response, cancellationToken)
                .ConfigureAwait(false);

            return responses;
        }

        public virtual async Task<IEnumerable<T>> Execute(
            IEnumerable<T> value,
            CancellationToken cancellationToken = default)
        {
            using var context = ContextFactory.Create();

            try
            {
                if (!await value
                    .AddToContextAsDeleted(context, cancellationToken)
                    .ConfigureAwait(false))
                {
                    return Enumerable.Empty<T>();
                }

                var results = await context
                    .SaveAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (results >= value.Count())
                {
                    return value;
                }

                return Enumerable.Empty<T>();
            }
            catch (ContextException exception)
            {
                throw new Exception(
                    $"{nameof(SqlDeleter<TContext, T>)}.{nameof(Delete)}: {exception.Message}",
                    exception);
            }
        }

        protected abstract Task<IEnumerable<T>> IsValid(
            IEnumerable<T> value,
            CancellationToken cancellationToken = default);

        protected abstract Task<IEnumerable<T>> Handle(
            IEnumerable<T> value,
            CancellationToken cancellationToken = default);
    }
}