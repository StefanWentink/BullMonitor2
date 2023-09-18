using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Exceptions;
using SWE.Infrastructure.Sql.Extensions;

namespace SWE.Infrastructure.Sql.Contracts
{
    public abstract class SqlBaseCreator<TContext, T>
        : ICollectionAndSingleCreator<T>
        where TContext : DbContext
        where T : class
    {
        protected IFactory<TContext> ContextFactory { get; }

        public SqlBaseCreator(
            IFactory<TContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public virtual async Task<T> Create(
            T value,
            CancellationToken cancellationToken = default)
        {
            var response = await Create(
                    new HashSet<T> { value },
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Single();
        }

        public virtual async Task<IEnumerable<T>> Create(
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
                    .AddToContextAsAdded(context, cancellationToken)
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
            catch (SqlException sqlException)
            {
                throw new ContextException(
                    $"{nameof(SqlCreator<TContext, T>)}.{nameof(Create)}: {sqlException.Message}, STACK => {sqlException.StackTrace}",
                    sqlException);
            }
            catch (DbUpdateException sqlException)
            {
                throw new ContextException(
                    $"{nameof(SqlCreator<TContext, T>)}.{nameof(Create)}: { sqlException.Message}, STACK => { sqlException.StackTrace }",
                    sqlException);
            }
            catch (ContextException exception)
            {
                throw new ContextException(
                    $"{nameof(SqlCreator<TContext, T>)}.{nameof(Create)}: {exception.Message}, STACK => {exception.StackTrace}",
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