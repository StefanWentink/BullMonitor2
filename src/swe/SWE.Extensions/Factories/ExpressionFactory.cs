using System.Linq.Expressions;

namespace SWE.Extensions.Factories
{
    public static class ExpressionFactory
    {
        public static Expression<Func<TValue, bool>> InExpression<TValue>(
            IEnumerable<TValue> values)
            where TValue : IEquatable<TValue>
        {
            return x => values != null && values.Contains(x);
        }

        public static Expression<Func<TValue, bool>> LessThanOrEqualExpression<TValue>(
            TValue value)
            where TValue : IComparable<TValue>
        {
            return x => value.CompareTo(x) <= 0;
        }

        public static Expression<Func<TValue, bool>> GreaterThanOrEqualExpression<TValue>(
            TValue value)
            where TValue : IComparable<TValue>
        {
            return x => value.CompareTo(x) >= 0;
        }

        public static Expression<Func<TValue, bool>> LessThanExpression<TValue>(
            TValue value)
            where TValue : IComparable<TValue>
        {
            return x => value.CompareTo(x) < 0;
        }

        public static Expression<Func<TValue, bool>> GreaterThanExpression<TValue>(
            TValue value)
            where TValue : IComparable<TValue>
        {
            return x => value.CompareTo(x) > 0;
        }

        public static Expression<Func<TValue, bool>> EqualExpression<TValue>(
            TValue value)
            where TValue : IComparable
        {
            return x => x.Equals(value);
        }

        public static Expression<Func<TValue, bool>> ContainsExpression<TValue>(
            IEnumerable<TValue> values)
            where TValue : IComparable
        {
            return x => values.Contains(x);
        }

        //public static Expression<Func<TValue, bool>>? InRangeExpression<TValue>(
        //    Range<TValue> value)
        //    where TValue : IComparable<TValue>
        //{
        //    Expression<Func<TValue, bool>>? upperExpression = null;
        //    Expression<Func<TValue, bool>>? lowerExpression = null;

        //    if (value.HasLowerBound)
        //    {
        //        lowerExpression = value.LowerBoundType.Equals(BoundaryType.Closed)
        //            ? (x => value.LowerBound.CompareTo(x) >= 0)
        //            : (x => value.LowerBound.CompareTo(x) > 0);
        //    }

        //    if (value.HasUpperBound)
        //    {
        //        upperExpression = value.UpperBoundType.Equals(BoundaryType.Closed)
        //            ? (x => value.UpperBound.CompareTo(x) >= 0)
        //            : (x => value.UpperBound.CompareTo(x) > 0);
        //    }

        //    return (lowerExpression == default && upperExpression == default)
        //        ? default
        //        : upperExpression.And(lowerExpression);
        //}

        //public static Boundary<TValue>? LowerBoundaryConvertExpression<TValue, TBoundary>(
        //    Boundary<TBoundary>? value,
        //    Func<TBoundary, TValue> convertFunction)
        //    where TValue : struct, IComparable<TValue>
        //    where TBoundary : IComparable<TBoundary>
        //{
        //    Boundary<TValue> boundary = default;

        //    if (value.HasValue)
        //    {
        //        boundary = new Boundary<TValue>(convertFunction(value.Value.Bound), value.Value.BoundaryType);
        //    }

        //    return boundary;
        //}

        //public static Expression<Func<TValue, bool>>? LowerBoundaryExpression<TValue>(
        //    Boundary<TValue>? value)
        //    where TValue : struct, IComparable<TValue>
        //{
        //    if (value.HasValue)
        //    {
        //        //lower bound < until, no matter what boundary type, because until is defined open
        //        return LessThanExpression(value.Value.Bound);
        //    }
        //    return default;
        //}

        //public static Boundary<TValue>? UpperBoundaryConvertExpression<TValue, TBoundary>(
        //    Boundary<TBoundary>? value,
        //    Func<TBoundary, TValue> convertFunction)
        //    where TValue : IComparable<TValue>
        //    where TBoundary : IComparable<TBoundary>
        //{
        //    Boundary<TValue> boundary = default;

        //    if (value.HasValue)
        //    {
        //        boundary = new Boundary<TValue>(convertFunction(value.Value.Bound), value.Value.BoundaryType);
        //    }

        //    return boundary;
        //}

        //public static Expression<Func<TValue, bool>>? UpperBoundaryExpression<TValue>(
        //    Boundary<TValue>? value)
        //    where TValue : IComparable<TValue>
        //{
        //    if (value.HasValue)
        //    {
        //        //upper bound >= from, because from is defined closed
        //        //upper bound > from, because from is defined closed
        //        return value.Value.BoundaryType.Equals(BoundaryType.Closed)
        //            ? GreaterThanOrEqualExpression(value.Value.Bound)
        //            : GreaterThanExpression(value.Value.Bound);
        //    }

        //    return default;
        //}
    }
}