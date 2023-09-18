using SWE.Extensions.Factories;
using System.Linq.Expressions;

namespace SWE.Extensions.Extensions
{
    public static class CollectionExpressionExtensions
    {
        public static Expression<Func<T, bool>>? ToExpression<T, TValue>(
                this IEnumerable<TValue>? collection,
                Expression<Func<T, TValue>> propertySelector)
            where TValue : IComparable
        {
            return propertySelector
                .CreateEqualsOrContains(
                    collection,
                    x => x);
        }

        public static Expression<Func<T, bool>>? ToNullableExpression<T, TValue>(
                this IEnumerable<TValue?> collection,
                Expression<Func<T, TValue?>> propertySelector)
        {
            return propertySelector
                .CreateNullableEqualsOrContains(
                    collection);
        }

        public static Expression<Func<T, bool>>? ToComparableNullableExpression<T, TValue>(
            this IEnumerable<TValue?> collection,
            Expression<Func<T, TValue?>> propertySelector)
            where TValue : IComparable
        {
            return propertySelector
                .CreateNullableEqualsOrContains(
                    collection);
        }

        public static Expression<Func<T, bool>>? ToEnumExpression<T, TValue>(
                this IEnumerable<TValue>? collection,
                Expression<Func<T, TValue>> propertySelector)
            where TValue : Enum
        {
            return propertySelector
                .CreateEnumEqualsOrContains(
                    collection);
        }

        public static Expression<Func<T, bool>>? ToBoolExpression<T>(
                this IEnumerable<bool> collection,
                Expression<Func<T, bool>> trueExpression,
                Expression<Func<T, bool>> falseExpression)
        {
            if (collection == null)
            {
                return x => true;
            }

            var containsTrue = collection.Any(x => x);
            var containsFalse = collection.Any(x => !x);

            if (containsTrue)
            {
                if (containsFalse)
                {
                    return x => true;
                }

                return trueExpression;
            }

            return falseExpression;
        }

        public static Expression<Func<T, bool>>? ToExpression<T, TValue, THaving>(
                this IEnumerable<THaving> collection,
                Expression<Func<T, TValue>> propertySelector,
                Func<THaving, TValue> convertFunction)
            where TValue : IComparable
        {
            return propertySelector
                .CreateEqualsOrContains(
                    collection,
                    convertFunction);
        }

        //public static Expression<Func<T, bool>>? CreateEqualsOrContains<T, TValue, TCollection>(
        //    this Expression<Func<T, TValue>> propertySelector,
        //    IEnumerable<TCollection>? collection,
        //    Func<TCollection, TValue> convertFunction)
        //    where TValue : IComparable
        //{
        //    if (collection == null)
        //    {
        //        return default;
        //    }

        //    if (collection.Count() == 1)
        //    {
        //        return x => false;
        //    }

        //    if (collection.Count() == 1)
        //    {
        //        var value = convertFunction(collection.Single());

        //        var equalExpression = ExpressionFactory.EqualExpression(value);

        //        return Combine(
        //            propertySelector,
        //            equalExpression);
        //    }

        //    var values = collection
        //        .Select(x => convertFunction(x));

        //    var containsExpression = ExpressionFactory
        //        .ContainsExpression(values);

        //    return Combine(
        //            propertySelector,
        //            containsExpression);
        //}
    }
}
