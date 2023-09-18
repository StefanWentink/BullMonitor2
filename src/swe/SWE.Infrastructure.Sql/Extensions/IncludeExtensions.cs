using SWE.Infrastructure.Sql.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class IncludeExtensions
    {
        public static IQueryable<T> ResolveInclude<T>(
            this IQueryable<T> query,
            IEnumerable<IIncludeResolver<T>>? includes)
            where T : class
        {
            if (includes?.Any() == true)
            {
                query = includes
                    .Aggregate(query, (current, includeResolver) => query = includeResolver.Resolve(query));
            }

            return query;
        }
    }
}