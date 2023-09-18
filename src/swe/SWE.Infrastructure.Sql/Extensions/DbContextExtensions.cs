using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MoreLinq.Extensions;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Data;
using SWE.Infrastructure.Sql.Exceptions;
using System.Reflection;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class DbContextExtensions
    {
        private static readonly FieldInfo? dbContextDisposedFieldInfo = typeof(DbContext)?
                .GetField(
                    "_disposed",
                    BindingFlags.NonPublic | BindingFlags.Instance);

        //public static DbContext DetachAllEntities_2(this DbContext dbContext)
        //{
        //    var changedEntriesCopy = dbContext
        //        .ChangeTracker
        //        .Entries()
        //        .ToList();

        //    foreach (var entry in changedEntriesCopy)
        //        entry.State = EntityState.Detached;

        //    return dbContext;
        //}

        public static bool IsDisposed(this DbContext? context)
        {
            if (context?.Database == null)
            {
                return true;
            }

            if (dbContextDisposedFieldInfo?.GetValue(context) is bool _bool)
            {
                return _bool;
            }

            return true;
        }

        /// <summary>
        /// Calls <see cref="DbContext.SaveChangesAsync"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Returns the number of updated items</returns>
        public static async Task<int> SaveAsync(
            this DbContext context,
            CancellationToken cancellationToken)
        {
            try
            {
                return await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (InvalidOperationException exception)
            {
                throw new ContextException(exception.GetInnerMostException().Message, exception);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ContextException(exception.GetInnerMostException().Message, exception);
            }
            catch (DbUpdateException exception)
            {
                throw new ContextException(exception.GetInnerMostException().Message, exception);
            }
            catch (AggregateException exception)
            {
                throw new ContextException(exception.GetInnerMostException().Message, exception);
            }
        }

        public static Task<bool> AddToContextAsAdded<T>(
            this IEnumerable<T> entities,
            DbContext context,
            CancellationToken cancellationToken)
            where T : class
        {
            return entities
                .AddToContext(
                    context,
                    EntityState.Added,
                    cancellationToken);
        }

        public static Task<bool> AddToContextAsModified<T>(
            this IEnumerable<T> entities,
            DbContext context,
            CancellationToken cancellationToken)
            where T : class
        {
            return entities
                .AddToContext(
                    context,
                    EntityState.Modified,
                    cancellationToken);
        }

        public static Task<bool> AddToContextAsDeleted<T>(
            this IEnumerable<T> entities,
            DbContext context,
            CancellationToken cancellationToken)
            where T : class
        {
            return entities
                .AddToContext(
                    context,
                    EntityState.Deleted,
                    cancellationToken);
        }

        private static Task<bool> AddToContext<T>(
            this IEnumerable<T> entities,
            DbContext context,
            EntityState entityState,
            CancellationToken cancellationToken)
            where T : class
        {
            if (cancellationToken.IsCancellationRequested || !entities.Any())
            {
                return Task.FromResult(
                    false);
            }

            context
                .Set<T>()
                .AttachRange(entities);

            var isAdded = entityState.Equals(EntityState.Added);

            if (isAdded)
            {
                entities
                    .Where(x => context.Entry(x)?.State == EntityState.Unchanged)
                    .ForEach(x => context.Entry(x).State = EntityState.Added);

                var addedEntities = context
                    .ChangeTracker
                    .Entries()
                    .Where(x =>
                        x.State.Equals(EntityState.Added)
                        && x is IId _idEntity)
                    .Select(x => x as IId);

                addedEntities
                    .Where(x => x?.Id.Equals(Guid.Empty) == true)
                    .ForEach(x => x.Id = Guid.NewGuid());
            }
            else
            {
                entities
                    .Where(x => context.Entry(x)?.State == EntityState.Unchanged)
                    .ForEach(x => context.Entry(x).State = entityState);
            }

            return Task.FromResult(
                context.HasProcessedEntities() || isAdded);
        }

        private static bool HasProcessedEntities(this DbContext context)
        {
            var addedEntities = context
                .ChangeTracker
                .Entries()
                .Where(x => x.State.Equals(EntityState.Added))
                .ToList();

            var modifiedEntities = context
                .ChangeTracker
                .Entries()
                .Where(x => x.State.Equals(EntityState.Modified))
                .ToList();

            var deletedEntities = context
                .ChangeTracker
                .Entries()
                .Where(x => x.State.Equals(EntityState.Deleted))
                .ToList();

            var unchangedEntities = context
                .ChangeTracker
                .Entries()
                .Where(x => x.State.Equals(EntityState.Unchanged))
                .ToList();

            OverrideUnchangedEntitiesAsAdded(unchangedEntities);

            return addedEntities.Count > 0
                || modifiedEntities.Count > 0
                || deletedEntities.Count > 0
                || unchangedEntities.Count > 0;
        }

        private static void OverrideUnchangedEntitiesAsAdded(List<EntityEntry> unchangedEntities)
        {
            unchangedEntities.ForEach(ee => ee.State = EntityState.Added);
        }

        public static void DetachAllEntities(
            this DbContext context)
        {
            foreach (var entry in context
                .ChangeTracker
                .Entries()
                .Where(x =>
                    x.Entity != null
                    && x.State != EntityState.Detached))
            {
                if (entry.Entity != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
    }
}