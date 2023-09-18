using MoreLinq;
using SWE.Infrastructure.Abstractions.Interfaces.Requests;
using System.Linq.Expressions;

namespace SWE.Infrastructure.Abstractions.Requests
{
    public class ConditionContainer<T>
        : IConditionContainer<T>
    {
        public ICollection<Expression<Func<T, bool>>> Expressions { get; } = new HashSet<Expression<Func<T, bool>>>();

        public ConditionContainer()
        { }

        public ConditionContainer(
            IConditionContainer<T> conditionContainer)
        {
            AddExpressions(conditionContainer.Expressions);
        }

        public ConditionContainer(
            Expression<Func<T, bool>> expression)
        {
            AddExpression(expression);
        }

        public ConditionContainer(
            IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            AddExpressions(expressions);
        }

        public void AddExpression(Expression<Func<T, bool>>? expression)
        {
            if (expression != null)
            {
                Expressions.Add(expression);
            }
        }

        public void AddExpressions(IEnumerable<Expression<Func<T, bool>>?>? expressions)
        {
            expressions?.ForEach(AddExpression);
        }
    }
}