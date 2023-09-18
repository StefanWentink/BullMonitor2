using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SWE.Infrastructure.Sql.Interfaces;

namespace SWE.Infrastructure.Sql.Models
{
    public class IncludeResolver<T, T2>
        : IIncludeResolver<T>
        where T : class
    {
        public IncludeResolver(
            Expression<Func<T, IQueryable<T2>>>? includeExpression,
            IIncludeResolver<T2>? nextInclude = null)
            : this(
                null,
                null,
                includeExpression,
                nextInclude)
        { }

        public IncludeResolver(
            Expression<Func<T, IEnumerable<T2>>>? includeExpression,
            IIncludeResolver<T2>? nextInclude = null)
            : this(
                null,
                includeExpression,
                null,
                nextInclude)
        { }

        public IncludeResolver(
            Expression<Func<T, T2>>? includeExpression,
            IIncludeResolver<T2>? nextInclude = null)
            : this(
                includeExpression,
                null,
                null,
                nextInclude)
        { }

        internal IncludeResolver(
            Expression<Func<T, T2>>? includeSingleExpression,
            Expression<Func<T, IEnumerable<T2>>>? includeEnumerableExpression,
            Expression<Func<T, IQueryable<T2>>>? includeQueryableExpression,
            IIncludeResolver<T2>? nextInclude)
        {
            IncludeSingleExpression = includeSingleExpression;
            IncludeEnumerableExpression = includeEnumerableExpression;
            IncludeQueryableExpression = includeQueryableExpression;
            NextInclude = nextInclude;
        }

        public Expression<Func<T, T2>>? IncludeSingleExpression { get; }
        public Expression<Func<T, IEnumerable<T2>>>? IncludeEnumerableExpression { get; }
        public Expression<Func<T, IQueryable<T2>>>? IncludeQueryableExpression { get; }

        public IIncludeResolver<T2>? NextInclude { get; }

        public IQueryable<T> Resolve(IQueryable<T> query)
        {
            if (IncludeSingleExpression != null)
            {
                var includeQuery = query
                    .Include(IncludeSingleExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeEnumerableExpression != null)
            {
                var includeQuery = query
                    .Include(IncludeEnumerableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeQueryableExpression != null)
            {
                var includeQuery = query
                    .Include(IncludeQueryableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            return query;
        }

        public IQueryable<TSource> Resolve<TSource>(
            IIncludableQueryable<TSource, T> query)
            where TSource : class
        {
            if (IncludeSingleExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeSingleExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeEnumerableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeEnumerableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeQueryableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeQueryableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            return query;
        }

        public IQueryable<TSource> Resolve<TSource>(
            IIncludableQueryable<TSource, IQueryable<T>> query)
            where TSource : class
        {
            if (IncludeSingleExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeSingleExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeEnumerableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeEnumerableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeQueryableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeQueryableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            return query;
        }

        public IQueryable<TSource> Resolve<TSource>(
            IIncludableQueryable<TSource, IEnumerable<T>> query)
            where TSource : class
        {
            if (IncludeSingleExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeSingleExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeEnumerableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeEnumerableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            if (IncludeQueryableExpression != null)
            {
                var includeQuery = query
                    .ThenInclude(IncludeQueryableExpression);

                if (NextInclude != null)
                {
                    return NextInclude.Resolve(includeQuery);
                }

                return includeQuery;
            }

            return query;
        }
    }
}