using SWE.Infrastructure.Abstractions.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SWE.Infrastructure.Sql.Interfaces
{
    public interface ISqlConditionContainer<T>
        : IConditionContainer<T>
    {
        void AddInclude(IIncludeResolver<T>? include);
        void AddInclude<T2>(Expression<Func<T, IEnumerable<T2>>>? include);
        void AddInclude<T2>(Expression<Func<T, IQueryable<T2>>>? include);
        void AddInclude<T2>(Expression<Func<T, T2>>? include);
        // Might not be needed
        //void AddCollectionInclude<T2>(Expression<Func<T, IEnumerable<T2>>>? include);

        //void AddSingleInclude(Expression<Func<T, dynamic>>? include);
        //void AddSingleIncludes(IEnumerable<Expression<Func<T, dynamic>>?> includes);

        void AddOrder(Expression<Func<T, dynamic>> order, bool asc = true);
        void AddOrders(IEnumerable<(Expression<Func<T, dynamic>> exp, bool asc)> orders);

        void SetSkip(int skip);
        void SetTake(int take);

        ICollection<IIncludeResolver<T>> Includes { get; }
        ICollection<(Expression<Func<T, dynamic>> exp, bool asc)> Orders { get; }
        int Skip { get; }
        int Take { get; }
    }
}