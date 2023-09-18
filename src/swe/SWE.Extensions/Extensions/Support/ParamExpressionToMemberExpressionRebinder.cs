using System.Linq.Expressions;

namespace SWE.Extensions.Extensions.Support
{

    internal class ParameterExpressionToMemberExpressionRebinder
        : ExpressionVisitor
    {
        private ParameterExpression ParameterExpression { get; }

        private Expression MemberExpression { get; }

        internal ParameterExpressionToMemberExpressionRebinder(
            ParameterExpression parameterExpression,
            Expression memberExpression)
        {
            ParameterExpression = parameterExpression;
            MemberExpression = memberExpression;
        }

        /// <summary>
        /// Rebind param expression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override Expression? Visit(Expression? node)
        {
            return base
                .Visit(
                    node == default || node == ParameterExpression
                        ? MemberExpression
                        : node);
        }
    }
}