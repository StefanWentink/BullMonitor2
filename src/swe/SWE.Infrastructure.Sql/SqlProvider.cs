using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using System.Linq.Expressions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Requests;

namespace SWE.Infrastructure.Sql
{
    public abstract class SqlProvider<T, TContext, TKey>
        : SqlProvider<T, TContext>
        where T : class
        where TContext : DbContext, IDisposable
    {
        protected SqlProvider(
            IFactory<TContext> contextFactory,
            IServiceProvider serviceProvider,
            ILogger<SqlProvider<T, TContext, TKey>> logger)
            : base(
                   contextFactory,
                   serviceProvider,
                   logger)
        { }

        protected SqlProvider(
           IFactory<TContext> contextFactory,
           ILogger<SqlProvider<T, TContext, TKey>> logger)
           : base(
                  contextFactory,
                  null,
                  logger)
        { }

        protected abstract Expression<Func<T, bool>> GetIdExpression(TKey id);
    }

    public class SqlProvider<T, TContext>
        : ISqlProvider<T>
        where T : class
        where TContext : DbContext
    {
        private readonly SemaphoreSlim _lock = new(1);

        protected IFactory<TContext> ContextFactory { get; }

        protected IServiceProvider? ServiceProvider { get; }

        protected ILogger<SqlProvider<T, TContext>> Logger { get; }

        public SqlProvider(
            IFactory<TContext> contextFactory,
            IServiceProvider? serviceProvider,
            ILogger<SqlProvider<T, TContext>> logger)
        {
            ContextFactory = contextFactory;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public virtual async Task<IEnumerable<T>> Get(
            CancellationToken cancellationToken)
        {
            return await Query(
                    new SqlConditionContainer<T>(),
                    EntityFrameworkQueryableExtensions.ToListAsync,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Get(
            ISqlConditionContainer<T> request,
            CancellationToken cancellationToken)
        {
            return await Query(
                    request,
                    EntityFrameworkQueryableExtensions.ToListAsync,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<T> GetSingle(
            ISqlConditionContainer<T> request,
            CancellationToken cancellationToken)
        {
            return await Query(
                    request,
                    EntityFrameworkQueryableExtensions.SingleAsync,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<T?> GetSingleOrDefault(
            ISqlConditionContainer<T> request,
            CancellationToken cancellationToken)
        {
            return await Query(
                    request,
                    EntityFrameworkQueryableExtensions.SingleOrDefaultAsync,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<long> GetCount(
            ISqlConditionContainer<T> request,
            CancellationToken cancellationToken)
        {
            return await Query(request, EntityFrameworkQueryableExtensions.LongCountAsync, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetExists(
            ISqlConditionContainer<T> request,
            CancellationToken cancellationToken)
        {
            var response = await Query(
                request,
                    EntityFrameworkQueryableExtensions.CountAsync,
                    0,
                    1,
                    cancellationToken)
                .ConfigureAwait(false);

            return response > 0;
        }

        protected async Task<TResponse> Query<TResponse>(
            ISqlConditionContainer<T> request,
            Func<IQueryable<T>, CancellationToken, Task<TResponse>> resolver,
            CancellationToken cancellationToken)
        {
            return await Query(
                    request,
                    resolver,
                    request.Skip,
                    request.Take,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<TResponse> Query<TResponse>(
            ISqlConditionContainer<T> request,
            Func<IQueryable<T>, CancellationToken, Task<TResponse>> resolver,
            int skip,
            int take,
            CancellationToken cancellationToken)
        {
            return await Query(
                    request.Expressions.And(),
                    request.Includes,
                    request.Orders,
                    resolver,
                    skip,
                    take,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<TResponse> Query<TResponse>(
            Expression<Func<T, bool>>? condition,
            Func<IQueryable<T>, CancellationToken, Task<TResponse>> resolver,
            CancellationToken cancellationToken)
        {
            return await Query(
                    condition,
                    Enumerable.Empty<IIncludeResolver<T>>(),
                    null,
                    resolver,
                    0,
                    0,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<TResponse> Query<TResponse>(
            Expression<Func<T, bool>>? condition,
            IEnumerable<IIncludeResolver<T>>? includes,
            Func<IQueryable<T>, CancellationToken, Task<TResponse>> resolver,
            CancellationToken cancellationToken)
        {
            return await Query(
                    condition,
                    includes,
                    null,
                    resolver,
                    0,
                    0,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task<TResponse> Query<TResponse>(
            Expression<Func<T, bool>>? condition,
            IEnumerable<IIncludeResolver<T>>? includes,
            IEnumerable<(Expression<Func<T, dynamic>> exp, bool asc)>? orders,
            Func<IQueryable<T>, CancellationToken, Task<TResponse>> resolver,
            int skip,
            int take,
            CancellationToken cancellationToken)
        {
            TContext? contextCount = null;

            string? queryString = null;

            try
            {
                await _lock
                    .WaitAsync(cancellationToken)
                    .ConfigureAwait(false);

                var countRequest = ServiceProvider?.GetService<ICountRequest>();

                Task<long>? countTask = null;

                using var context = ContextFactory.Create();

                var query = context
                    .Set<T>()
                    .AsQueryable();

                query = query
                    .Apply(
                        condition,
                        includes);

                if (take > 0 && countRequest != null)
                {
                    contextCount = ContextFactory.Create();

                    var queryCount = contextCount
                        .Set<T>()
                        .AsQueryable();

                    queryCount = queryCount
                        .Apply(
                            condition,
                            includes);

                    countTask = EntityFrameworkQueryableExtensions.LongCountAsync(queryCount, CancellationToken.None);
                }

                var orderCollection = orders
                    ?? Enumerable.Empty<(Expression<Func<T, dynamic>> expression, bool asc)>();

                if (orderCollection.Any())
                {
                    IOrderedQueryable<T>? orderedQuery = null;

                    foreach (var (exp, asc) in orderCollection)
                    {
                        if (asc)
                        {
                            orderedQuery = orderedQuery == null
                                ? query.OrderBy(exp)
                                : orderedQuery.ThenBy(exp);
                        }
                        else
                        {
                            orderedQuery = orderedQuery == null
                                ? query.OrderByDescending(exp)
                                : orderedQuery.ThenByDescending(exp);
                        }
                    }

                    query = orderedQuery ?? query;
                }

                if (skip > 0)
                {
                    query = query
                        .Skip(skip);
                }

                if (take > 0)
                {
                    query = query
                        .Take(take);
                }

#if DEBUG
                try
                {
                    var connectionString = context.Database.GetDbConnection().ConnectionString;
                    queryString = query.ToQueryString();
                    Logger.LogDebug($"{ connectionString }");
                    Logger.LogDebug($"{nameof(query)} : {queryString}");
                }
                catch (AggregateException aggregateException)
                {
                    Logger?.LogError(aggregateException, aggregateException.Message, default);
                    throw;
                }
#endif

                var result = await resolver(query, cancellationToken)
                    .ConfigureAwait(false);

                if (countTask != null && countRequest != null)
                {
                    countRequest.Count = await countTask.ConfigureAwait(false);
                }

                return result;
            }
            catch (AggregateException aggregateException)
            {
                Logger?.LogError(aggregateException, aggregateException.Message, default);
                throw;
            }
            catch (Exception exception)
            {
                Logger?.LogError(exception, exception.Message, default);
                Logger?.LogError($"SQL_STRING:{queryString}", default);
                throw;
            }
            finally
            {
                contextCount?.Dispose();
                this._lock.Release();
            }
        }
    }
}