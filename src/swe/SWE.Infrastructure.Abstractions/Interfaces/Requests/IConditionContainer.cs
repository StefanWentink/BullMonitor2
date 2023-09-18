using System.Linq.Expressions;

namespace SWE.Infrastructure.Abstractions.Interfaces.Requests
{
    public interface IConditionContainer<T>
    {
        void AddExpression(Expression<Func<T, bool>>? expression);
        void AddExpressions(IEnumerable<Expression<Func<T, bool>>?>? expressions);

        ICollection<Expression<Func<T, bool>>> Expressions { get; }
    }
}