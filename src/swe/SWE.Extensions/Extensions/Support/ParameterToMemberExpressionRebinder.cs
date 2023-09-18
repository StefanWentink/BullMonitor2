using System.Linq.Expressions;

#nullable enable

namespace SWE.Extensions.Extensions.Support
{
    internal class ParameterToMemberExpressionRebinder
        : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly MemberExpression? _member;

        public ParameterToMemberExpressionRebinder(
            ParameterExpression parameter,
            MemberExpression? member)
        {
            _parameter = parameter;
            _member = member;
        }

        public override Expression? Visit(Expression? expression)
        {
            return base
                .Visit(
                expression == _parameter
                    ? _member
                    : expression);
        }
    }
}