using System.Linq.Expressions;

#nullable enable

namespace SWE.Extensions.Extensions.Support
{
    internal class ParameterToExpressionRebinder
            : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly Expression? _expression;

        public ParameterToExpressionRebinder(
            ParameterExpression parameter,
            Expression? expression)
        {
            _parameter = parameter;
            _expression = expression;
        }

        public override Expression? Visit(Expression? expression)
        {
            return base
                .Visit(
                    expression == _parameter
                        ? _expression
                        : expression);
        }
    }
}