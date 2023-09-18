using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Exceptions;
using SWE.Infrastructure.Sql.Interfaces;
using System.Linq.Expressions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Extensions.Extensions;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlMapUpserter<TContext, T>
        : SqlUpserter<TContext, T>
        where TContext : DbContext
        where T : class
    {
        protected IMapper<T, ISqlConditionContainer<T>> Mapper { get; }

        public SqlMapUpserter(
            IFactory<TContext> contextFactory,
            ICreator<IEnumerable<T>> creator,
            IUpdater<IEnumerable<T>> updater,
            IMapper<T, ISqlConditionContainer<T>> mapper)
            : base(
                  creator,
                  updater,
                  contextFactory)
        {
            Mapper = mapper;
        }

        protected override async Task<Expression<Func<T, bool>>> SelectExpression(
            T value,
            CancellationToken cancellationToken)
        {
            return
                (
                    await Mapper
                        .Map(value, cancellationToken)
                        .ConfigureAwait(false)
                ).Expressions
                .And();
        }
    }

    public abstract class SqlUpserter<TContext, T>
        : ICollectionAndSingleUpserter<T>
        where TContext : DbContext
        where T : class
    {
        protected ICreator<IEnumerable<T>> Creator { get; }
        protected IUpdater<IEnumerable<T>> Updater { get; }
        protected IFactory<TContext> ContextFactory { get; }

        protected SqlUpserter(
            ICreator<IEnumerable<T>> creator,
            IUpdater<IEnumerable<T>> updater,
            IFactory<TContext> contextFactory)
        {
            Creator = creator;
            Updater = updater;
            ContextFactory = contextFactory;
        }

        public async Task<T> Upsert(
            T value,
            CancellationToken cancellationToken)
        {
            var response = await Upsert(
                    new HashSet<T> { value },
                    cancellationToken)
                .ConfigureAwait(false);

            return response.Single();
        }

        public async Task<IEnumerable<T>> Upsert(
            IEnumerable<T> value,
            CancellationToken cancellationToken)
        {
            using var context = ContextFactory.Create();

            try
            {
                (IEnumerable<T> creates, IEnumerable<T> updates) = await SplitInCreateAndUpdate(
                        value,
                        context,
                        cancellationToken)
                    .ConfigureAwait(false);

                var createdTask = Creator.Create(creates, cancellationToken);
                var updatedTask = Updater.Update(updates, cancellationToken);

                var response = await Task
                    .WhenAll(new HashSet<Task<IEnumerable<T>>> { createdTask, updatedTask })
                    .ConfigureAwait(false);

                return response.SelectMany(x => x);
            }
            catch (ContextException exception)
            {
#if DEBUG
                Console.WriteLine(exception.Message);
                throw;
#endif
            }
        }

        protected abstract Task<Expression<Func<T, bool>>> SelectExpression(
            T value,
            CancellationToken cancellationToken);

        protected async Task<(IEnumerable<T> creates, IEnumerable<T> updates)> SplitInCreateAndUpdate(
            IEnumerable<T> values,
            TContext context,
            CancellationToken cancellationToken)
        {
            List<T> creates = new();
            List<T> updates = new();

            foreach (var value in values)
            {
                var count = await context
                    .Set<T>()
                    .CountAsync(await SelectExpression(value, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);

                if (count == 0)
                {
                    creates.Add(value);
                }
                else
                {
                    updates.Add(value);
                }
            }

            return (creates, updates);
        }
    }
}