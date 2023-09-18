using MoreLinq;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Requests;
using SWE.Infrastructure.Abstractions.Requests;
using SWE.Infrastructure.Sql.Interfaces;
using System.Linq.Expressions;

namespace SWE.Infrastructure.Sql.Models
{
    public class SqlConditionContainer<T>
        : ConditionContainer<T>
        , ISqlConditionContainer<T>
        where T : class
    {
        public ICollection<IIncludeResolver<T>> Includes { get; } = new HashSet<IIncludeResolver<T>>();

        public ICollection<(Expression<Func<T, dynamic>> exp, bool asc)> Orders { get; } = new HashSet<(Expression<Func<T, dynamic>> exp, bool asc)>();

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public SqlConditionContainer()
        { }

        public SqlConditionContainer(
            IConditionContainer<T> conditionContainer)
            : this(
                  conditionContainer.Expressions)
        { }

        public SqlConditionContainer(
            Expression<Func<T, bool>> expression)
            : this(
                 expression.Yield().ToList())
        { }

        public SqlConditionContainer(
            ICollection<Expression<Func<T, bool>>> expressions)
            : this(
                 expressions,
                 new HashSet<IIncludeResolver<T>>())
        { }

        public SqlConditionContainer(
            ISqlConditionContainer<T> container)
            : this(
                 container?.Expressions ?? new HashSet<Expression<Func<T, bool>>>(),
                 container?.Includes ?? new HashSet<IIncludeResolver<T>>())
        { }

        public SqlConditionContainer(
            ICollection<Expression<Func<T, bool>>> expressions,
            ICollection<IIncludeResolver<T>> includes)
            : base(
                 expressions)
        {
            Includes = includes ?? new HashSet<IIncludeResolver<T>>();
        }


        public void AddInclude(IIncludeResolver<T>? include)
        {
            if (include != null)
            {
                Includes.Add(include);
            }
        }

        public void AddInclude<T2>(Expression<Func<T, IQueryable<T2>>>? include)
        {
            AddInclude(new IncludeResolver<T, T2>(include));
        }

        public void AddInclude<T2>(Expression<Func<T, IEnumerable<T2>>>? include)
        {
            AddInclude(new IncludeResolver<T, T2>(include));
        }

        public void AddInclude<T2>(Expression<Func<T, T2>>? include)
        {
            AddInclude(new IncludeResolver<T, T2>(include));
        }

        public void AddOrder(Expression<Func<T, dynamic>> order, bool asc = true)
        {
            Orders.Add((order, asc));
        }

        public void AddOrders(IEnumerable<(Expression<Func<T, dynamic>> exp, bool asc)> orders)
        {
            orders.ForEach(x => AddOrder(x.exp, x.asc));
        }

        public void SetSkip(int skip)
        {
            if (skip >= 0)
            {
                Skip = skip;
            }
        }

        public void SetTake(int take)
        {
            if (take >= 0)
            {
                Take = take;
            }
        }
    }
}