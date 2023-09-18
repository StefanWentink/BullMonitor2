using SWE.Infrastructure.Sql.Extensions;
using SWE.Infrastructure.Sql.Interfaces;
using System.Linq.Expressions;

namespace SWE.Infrastructure.Sql
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Apply<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>>? condition,
            IEnumerable<IIncludeResolver<T>>? includes)
            where T : class
        {
            if (condition != default)
            {
                query = query
                    .Where(condition);
            }

            query = query
                .ResolveInclude(
                    includes);

            return query;
        }
    }
}