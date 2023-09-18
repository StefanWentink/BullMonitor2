using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace SWE.Infrastructure.Sql.Interfaces
{
    public interface IIncludeResolver<T>
    {
        IQueryable<T> Resolve(IQueryable<T> query);

        IQueryable<TSource> Resolve<TSource>(IIncludableQueryable<TSource, T> query)
            where TSource : class;

        IQueryable<TSource> Resolve<TSource>(IIncludableQueryable<TSource, IQueryable<T>> query)
            where TSource : class;

        IQueryable<TSource> Resolve<TSource>(IIncludableQueryable<TSource, IEnumerable<T>> query)
            where TSource : class;
    }
}