using SWE.Extensions.Extensions.Support;
using SWE.Extensions.Factories;
using System.Linq.Expressions;

namespace SWE.Extensions.Extensions
{
    public static class SelectorExpressionExtensions
    {
        public static Expression<Func<T, bool>> Combine<T, TValue>(
            this Expression<Func<T, TValue?>> propertySelector,
            Expression<Func<TValue, bool>> propertyPredicate)
            where TValue : struct, IComparable<TValue>
        {
            if (Nullable.GetUnderlyingType(propertySelector.Body.Type) != default)
            {
                MethodCallExpression? member = Expression.Call(propertySelector.Body, nameof(Nullable<TValue>.GetValueOrDefault), Type.EmptyTypes);

                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(propertyPredicate.Body, propertySelector.ToExpression().Parameters);
                ParameterToExpressionRebinder? rebinder = new ParameterToExpressionRebinder(propertyPredicate.Parameters[0], member);
                return (Expression<Func<T, bool>>)rebinder.Visit(expression);
            }
            else if (propertySelector.Body is MemberExpression _memberExpressions && !propertySelector.Type.Name.Contains("Nullable"))
            {
                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(propertyPredicate.Body, propertySelector.Parameters);
                ParameterToMemberExpressionRebinder rebinder = new ParameterToMemberExpressionRebinder(propertyPredicate.Parameters[0], _memberExpressions);
                return (Expression<Func<T, bool>>)rebinder.Visit(expression);
            }
            throw new ArgumentException($"{nameof(propertySelector)} is not a valid {nameof(MemberExpression)} or {nameof(Nullable<TValue>)}.");
        }

        public static Expression<Func<T, bool>> Combine<T, TValue>(
            this Expression<Func<T, TValue>> propertySelector,
            Expression<Func<TValue, bool>> propertyPredicate)
        {
            if (propertySelector.Body is MemberExpression _memberExpression)
            {
                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(propertyPredicate.Body, propertySelector.Parameters);
                ParameterToMemberExpressionRebinder rebinder = new ParameterToMemberExpressionRebinder(propertyPredicate.Parameters[0], _memberExpression);
                return (Expression<Func<T, bool>>)rebinder.Visit(expression);
            }

            throw new ArgumentException($"{nameof(propertySelector)} is not a valid {nameof(MemberExpression)}.");
        }

        public static Expression<Func<T, TValue>> ToExpression<T, TValue>(
           this Expression<Func<T, TValue?>> propertySelector)
            where TValue : struct
        {
            if (Nullable.GetUnderlyingType(propertySelector.Body.Type) == default)
            {
                Expression.Lambda<Func<T, TValue>>(propertySelector.Body, propertySelector.Parameters);
            }

            MethodCallExpression? callExpression = Expression.Call(propertySelector.Body, nameof(Nullable<TValue>.GetValueOrDefault), Type.EmptyTypes);
            return Expression.Lambda<Func<T, TValue>>(callExpression, propertySelector.Parameters);
        }

        //public static Expression<Func<T, bool>>? CreateHavingInRangeExpression<T, TValue>(
        //        Expression<Func<T, TValue?>> propertySelector,
        //        Range<TValue> range)
        //    where TValue : struct, IComparable<TValue>
        //{
        //    if (range.GuardIsRangeAny(typeof(T).Name))
        //    {
        //        return default;
        //    }

        //    return CreateHavingInRangeExpression(
        //        propertySelector,
        //        propertySelector,
        //        range.LowerDelimiter,
        //        range.UpperDelimiter);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeExpression<T, TValue>(
        //        Expression<Func<T, TValue?>> propertySelectorLow,
        //        Expression<Func<T, TValue?>> propertySelectorUp,
        //        Range<TValue> range)
        //    where TValue : struct, IComparable<TValue>
        //{
        //    if (range.GuardIsRangeAny(typeof(T).Name))
        //    {
        //        return default;
        //    }

        //    return CreateHavingInRangeExpression(
        //        propertySelectorLow,
        //        propertySelectorUp,
        //        range.LowerDelimiter,
        //        range.UpperDelimiter);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeConvertExpression<T, TValue, TRange>(
        //        Expression<Func<T, TValue?>> propertySelector,
        //        Range<TRange> range,
        //        Func<TRange, TValue> convertFunction)
        //    where TValue : struct, IComparable<TValue>
        //    where TRange : IComparable<TRange>
        //{
        //    if (range.GuardIsRangeAny(typeof(T).Name))
        //    {
        //        return default;
        //    }

        //    return CreateHavingInRangeConvertExpression(
        //        propertySelector,
        //        propertySelector,
        //        range.LowerDelimiter,
        //        range.UpperDelimiter,
        //        convertFunction);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeConvertExpression<T, TValue, TRange>(
        //        Expression<Func<T, TValue?>> propertySelectorLow,
        //        Expression<Func<T, TValue?>> propertySelectorUp,
        //        Range<TRange> range,
        //        Func<TRange, TValue> convertFunction)
        //    where TValue : struct, IComparable<TValue>
        //    where TRange : IComparable<TRange>
        //{
        //    if (range.GuardIsRangeAny(typeof(T).Name))
        //    {
        //        return default;
        //    }

        //    return CreateHavingInRangeConvertExpression(
        //        propertySelectorLow,
        //        propertySelectorUp,
        //        range.LowerDelimiter,
        //        range.UpperDelimiter,
        //        convertFunction);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeExpression<T, TValue>(
        //        Expression<Func<T, TValue?>> propertySelector,
        //        Boundary<TValue>? valueLow,
        //        Boundary<TValue>? valueUp)
        //    where TValue : struct, IComparable<TValue>
        //{
        //    return CreateHavingInRangeConvertExpression(
        //        propertySelector,
        //        valueLow,
        //        valueUp,
        //        x => x);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeExpression<T, TValue>(
        //        Expression<Func<T, TValue?>> propertySelectorLow,
        //        Expression<Func<T, TValue?>> propertySelectorUp,
        //        Boundary<TValue>? valueLow,
        //        Boundary<TValue>? valueUp)
        //    where TValue : struct, IComparable<TValue>
        //{
        //    return CreateHavingInRangeConvertExpression(
        //        propertySelectorLow,
        //        propertySelectorUp,
        //        valueLow,
        //        valueUp,
        //        x => x);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeConvertExpression<T, TValue, TRange>(
        //        Expression<Func<T, TValue?>> propertySelector,
        //        Boundary<TRange>? valueLow,
        //        Boundary<TRange>? valueUp,
        //        Func<TRange, TValue> convertFunction)
        //    where TValue : struct, IComparable<TValue>
        //    where TRange : IComparable<TRange>
        //{
        //    return CreateHavingInRangeConvertExpression(
        //        propertySelector,
        //        propertySelector,
        //        valueLow,
        //        valueUp,
        //        convertFunction);
        //}

        //public static Expression<Func<T, bool>>? CreateHavingInRangeConvertExpression<T, TValue, TRange>(
        //        Expression<Func<T, TValue?>> propertySelectorLow,
        //        Expression<Func<T, TValue?>> propertySelectorUp,
        //        Boundary<TRange>? valueLow,
        //        Boundary<TRange>? valueUp,
        //        Func<TRange, TValue> convertFunction)
        //    where TValue : struct, IComparable<TValue>
        //    where TRange : IComparable<TRange>
        //{
        //    Expression<Func<T, bool>>? expression = default;

        //    if (valueLow.HasValue)
        //    {
        //        Boundary<TValue>? boundary = ExpressionFactory.LowerBoundaryConvertExpression(valueLow, convertFunction);

        //        Expression<Func<TValue, bool>>? lowerParameterPredicate = ExpressionFactory.LowerBoundaryExpression(boundary);

        //        if (lowerParameterPredicate != null)
        //        {
        //            expression = Combine(propertySelectorUp, lowerParameterPredicate);
        //        }
        //    }
        //    if (valueUp.HasValue)
        //    {
        //        Boundary<TValue>? boundary = ExpressionFactory.UpperBoundaryConvertExpression<TValue, TRange>(valueUp, convertFunction);

        //        Expression<Func<TValue, bool>>? upperParameterPredicate = ExpressionFactory.UpperBoundaryExpression(boundary);

        //        if (upperParameterPredicate != null)
        //        {
        //            expression = expression.And(Combine(propertySelectorLow, upperParameterPredicate));
        //        }
        //    }

        //    return expression;
        //}

        public static Expression<Func<T, bool>>? CreateEqualsOrContains<T, TValue>(
                this Expression<Func<T, TValue>> propertySelector,
                IEnumerable<TValue>? collection)
            where TValue : IComparable
        {
            return propertySelector
                .CreateEqualsOrContains(
                    collection,
                    x => x);
        }

        public static Expression<Func<T, bool>>? CreateNullableEnumEqualsOrContains<T, TValue>(
                this Expression<Func<T, TValue?>> propertySelector,
                IEnumerable<TValue?>? collection)
            where TValue : IComparable
        {
            return propertySelector
                .CreateNullableEqualsOrContains(
                    collection);
        }

        public static Expression<Func<T, bool>>? CreateEqualsOrContains<T, TValue, THaving>(
                this Expression<Func<T, TValue>> propertySelector,
                IEnumerable<THaving>? collection,
                Func<THaving, TValue> convertFunction)
            where TValue : IComparable
        {
            if (collection == null)
            {
                return default;
            }

            if (collection.Count() == 1)
            {
                var value = convertFunction(collection.Single());

                var equalExpression = ExpressionFactory.EqualExpression(value);

                return Combine(
                    propertySelector,
                    equalExpression);
            }

            var values = collection
                .Select(x => convertFunction(x));

            var containsExpression = ExpressionFactory
                .ContainsExpression(values);

            return Combine(
                    propertySelector,
                    containsExpression);
        }

        public static Expression<Func<T, bool>>? CreateNullableEqualsOrContains<T, TValue>(
                this Expression<Func<T, TValue?>> propertySelector,
                IEnumerable<TValue?>? collection)
        {
            if (collection == null)
            {
                return default;
            }

            if (collection.Count() == 1)
            {
                var value = collection.Single();

                Expression<Func<TValue?, bool>> equalExpression = x =>
                    ((x == null) && (value == null))
                    || (x != null && x.Equals(value));

                return Combine(
                    propertySelector,
                    equalExpression);
            }

            return Combine(
                    propertySelector,
                    x => collection.Contains(x));
        }

        public static Expression<Func<T, bool>>? CreateEnumEqualsOrContains<T, TValue>(
            this Expression<Func<T, TValue>> propertySelector,
            IEnumerable<TValue>? collection)
            where TValue : Enum
        {
            if (collection == null)
            {
                return default;
            }

            if (collection.Count() == 1)
            {
                var value = collection.Single();

                Expression<Func<TValue, bool>> equalExpression = x => value.Equals(x);

                return Combine(
                    propertySelector,
                    equalExpression);
            }

            Expression<Func<TValue, bool>> containsExpression = x => collection.Contains(x);

            return Combine(
                    propertySelector,
                    containsExpression);
        }

        public static Expression<Func<T, bool>>? CreateEndsWith<T, TValue>(
            this Expression<Func<T, TValue>> propertySelector,
            string value)
            where TValue : IComparable
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            return Combine(
                propertySelector,
                x => x.ToString().EndsWith(value));
        }
    }
}