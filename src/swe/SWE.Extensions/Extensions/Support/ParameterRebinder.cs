namespace SWE.Extensions.Extensions.Support
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal class ParameterRebinder
        : ExpressionVisitor
    {
        private Dictionary<ParameterExpression, ParameterExpression> Map { get; }

        internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            Map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (Map.TryGetValue(node, out var replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}