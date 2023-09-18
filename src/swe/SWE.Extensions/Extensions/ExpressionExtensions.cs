using SWE.Extensions.Extensions.Support;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace SWE.Extensions.Extensions
{
    public static class ExpressionExtensions
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> _whereMethodInfoDictionary = new();
        private static readonly ConcurrentDictionary<Type, MethodInfo> _anyMethodInfoDictionary = new();
        private static readonly SemaphoreSlim _whereMethodInfoDictionaryLock = new(1);
        private static readonly SemaphoreSlim _anyMethodInfoDictionaryLock = new(1);

        /// <summary>
        /// Applies a type based <see cref="propertySelector"/> and <see cref="propertyPredicate"/> to a single expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="propertySelector"></param>
        /// <param name="propertyPredicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When <paramref name="propertySelector"/> not <see cref="MemberExpression"/>.</exception>
        public static Expression<Func<T, TResult>> CombineSelectorParamExpression<T, TSelector, TResult>(
            this Expression<Func<T, TSelector>> propertySelector,
            Expression<Func<TSelector, TResult>> propertyPredicate)
        {
            if (propertySelector.Body is not MemberExpression memberExpression)
            {
                throw new ArgumentException(
                    $"{nameof(propertySelector)} is not a { typeof(MemberExpression).Name}.",
                    nameof(propertySelector));
            }

            var rebinder = new ParameterExpressionToMemberExpressionRebinder(propertyPredicate.Parameters[0], memberExpression);
            var expression = Expression.Lambda<Func<T, TResult>>(propertyPredicate.Body, propertySelector.Parameters);

            return (Expression<Func<T, TResult>>)rebinder.Visit(expression);
        }

        /// <summary>
        /// Combining including conditions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="selector"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationExceptionWithSubject">When <see cref="MethodInfo"/> could not be stored.</exception>
        public static Expression<Func<T, IEnumerable<TChild>>> CombineInclude<T, TChild>(
                this Expression<Func<T, IEnumerable<TChild>>> selector,
                Expression<Func<TChild, bool>> condition)
            where T : class
            where TChild : class
        {
            // Consider caching the MethodInfo object: 
            var whereMethodInfo = GetEnumerableWhereMethodInfo<TChild>();

            // Produce an Expression tree like:
            // Enumerable.Where(<navigationProperty>, <filter>)
            var filterExpr = Expression
                .Call(
                    whereMethodInfo,
                    selector.Body,
                    condition);

            // Create a Lambda Expression with the parameters
            // used for `navigationProperty` expression
            return Expression
                .Lambda<Func<T, IEnumerable<TChild>>>(
                    filterExpr,
                    selector.Parameters
                );
        }

        /// <summary>
        /// Combining including conditions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="selector"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationExceptionWithSubject">When <see cref="MethodInfo"/> could not be stored.</exception>
        public static Expression<Func<T, bool>> CombineFilter<T, TChild>(
                this Expression<Func<T, IEnumerable<TChild>>> selector,
                Expression<Func<TChild, bool>> condition)
            where T : class
            where TChild : class
        {
            // Consider caching the MethodInfo object: 
            var anyMethodInfo = GetEnumerableAnyMethodInfo<TChild>();

            // Produce an Expression tree like:
            // Enumerable.Any(<navigationProperty>, <filter>)
            var filterExpr = Expression
                .Call(
                    anyMethodInfo,
                    selector.Body,
                    condition);

            // Create a Lambda Expression with the parameters
            // used for `navigationProperty` expression
            return Expression
                .Lambda<Func<T, bool>>(
                    filterExpr,
                    selector.Parameters
                );
        }

        private static MethodInfo GetEnumerableWhereMethodInfo<TSource>()
        {
            try
            {
                var type = typeof(TSource);

                _whereMethodInfoDictionaryLock.Wait(1000);

                if (_whereMethodInfoDictionary.TryGetValue(type, out var _methodInfo))
                {
                    return _methodInfo;
                }

                // Get a MethodInfo definition for `Enumerable.Where<>`:
                var methodInfoDefinition = typeof(Enumerable)
                   .GetMethods()
                   .Where(x => x.Name == nameof(Enumerable.Where))
                   .First(x =>
                   {
                       var parameters = x.GetParameters();

                       return
                           parameters.Length == 2 &&
                           parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                           parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>);
                   });

                // Get a MethodInfo object for `Enumerable.Where<TSource>`:
                var methodInfo = methodInfoDefinition.MakeGenericMethod(typeof(TSource));

                if (_whereMethodInfoDictionary.TryAdd(type, methodInfo))
                {
                    return methodInfo;
                }

                throw new InvalidOperationException(
                    $"{nameof(ExpressionExtensions)}.{nameof(GetEnumerableWhereMethodInfo)}: Could not store {nameof(MethodInfo)} for {nameof(Enumerable.Where)} of {nameof(type)} '{type.Name}'.");
            }
            finally
            {
                _whereMethodInfoDictionaryLock.Release();
            }
        }

        private static MethodInfo GetEnumerableAnyMethodInfo<TSource>()
        {
            try
            {
                var type = typeof(TSource);

                _anyMethodInfoDictionaryLock.Wait(1000);

                if (_anyMethodInfoDictionary.TryGetValue(type, out var _methodInfo))
                {
                    return _methodInfo;
                }

                // Get a MethodInfo definition for `Enumerable.Any<>`:
                var methodInfoDefinition = typeof(Enumerable)
                   .GetMethods()
                   .Where(x => x.Name == nameof(Enumerable.Any))
                   .First(x =>
                   {
                       var parameters = x.GetParameters();

                       return
                           parameters.Length == 2 &&
                           parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                           parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>);
                   });

                // Get a MethodInfo object for `Enumerable.Any<TSource>`:
                var methodInfo = methodInfoDefinition.MakeGenericMethod(typeof(TSource));

                if (_anyMethodInfoDictionary.TryAdd(type, methodInfo))
                {
                    return methodInfo;
                }

                throw new InvalidOperationException(
                    $"{nameof(ExpressionExtensions)}.{nameof(GetEnumerableAnyMethodInfo)}: Could not store {nameof(MethodInfo)} for {nameof(Enumerable.Any)} of {nameof(type)} '{type.Name}'.");
            }
            finally
            {
                _anyMethodInfoDictionaryLock.Release();
            }
        }

        /// <summary>
        /// Combine an expression collection with the <see cref="And{T}"/> operator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>>? And<T>(
            this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            if (expressions?.Any() != true)
            {
                return default;
            }

            if (expressions?.Count() == 1)
            {
                return expressions.Single();
            }

            return expressions?
                .Aggregate((x, y) => x.And(y));
        }

        /// <summary>
        /// Combine an expression collection with the <see cref="Or{T}"/> operator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>>? Or<T>(
            this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            if (expressions?.Any() != true)
            {
                return default;
            }
            if (expressions?.Count() == 1)
            {
                return expressions.Single();
            }

            return expressions?
                .Aggregate((x, y) => x.Or(y));
        }

        /// <summary>
        /// Combine two expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="additionalExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<T, bool>> additionalExpression)
        {
            return expression
                .Combine(additionalExpression, false);
        }

        /// <summary>
        /// Combine two expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="additionalExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<T, bool>> additionalExpression)
        {
            return expression
                .Combine(additionalExpression, true);
        }

        private static Expression<Func<T, bool>> Combine<T>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<T, bool>> additionalExpression, bool or)
        {
            if (expression == default)
            {
                return additionalExpression;
            }

            if (additionalExpression == default)
            {
                return expression;
            }

            return or
                ? expression.ComposeOr(additionalExpression)
                : expression.ComposeAnd(additionalExpression);
        }

        /// <summary>
        /// Combine two expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="additionalExpression"></param>
        /// <returns></returns>
        public static Expression<Func<TResult, bool>> AndSub<T, TResult>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<TResult, bool>> additionalExpression)
            where TResult : T
        {
            return expression.CombineSub(additionalExpression, false);
        }

        /// <summary>
        /// Combine two expressions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <param name="additionalExpression"></param>
        /// <returns></returns>
        public static Expression<Func<TResult, bool>> OrSub<T, TResult>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<TResult, bool>> additionalExpression)
            where TResult : T
        {
            return expression.CombineSub(additionalExpression, true);
        }

        private static Expression<Func<TResult, bool>> CombineSub<T, TResult>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<TResult, bool>> additionalExpression,
            bool or)
            where TResult : T
        {
            if (expression == default)
            {
                return additionalExpression;
            }

            return or
                ? expression.ComposeOrSub(additionalExpression)
                : expression.ComposeAndSub(additionalExpression);
        }

        private static Expression<Func<T, bool>> ComposeAnd<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        private static Expression<Func<TResult, bool>> ComposeAndSub<T, TResult>(this Expression<Func<T, bool>> first, Expression<Func<TResult, bool>> second)
            where
            TResult : T
        {
            return ComposeSub(first, second, true);
        }

        private static Expression<Func<T, bool>> ComposeOr<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        private static Expression<Func<TResult, bool>> ComposeOrSub<T, TResult>(this Expression<Func<T, bool>> first, Expression<Func<TResult, bool>> second)
            where TResult : T
        {
            return first.ComposeSub(second, false);
        }

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        private static Expression<Func<TResult, bool>> ComposeSub<T, TResult>(
            this Expression<Func<T, bool>> expression,
            Expression<Func<TResult, bool>> additionalExpression,
            bool and)
            where TResult : T
        {
            var parameter = Expression.Parameter(typeof(TResult));

            var leftVisitor = new ReplaceExpressionVisitor(expression.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression.Body);

            var secondaryExpression = additionalExpression;
            var expressionOperatorAnd = and;

            if (secondaryExpression == default)
            {
                secondaryExpression = _ => true;
                expressionOperatorAnd = true;
            }

            var rightVisitor = new ReplaceExpressionVisitor(secondaryExpression.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondaryExpression.Body);

            if (left == default)
            {
                return secondaryExpression;
            }
            if (right == default)
            {
                return secondaryExpression;
            }

            return Expression
                .Lambda<Func<TResult, bool>>(
                expressionOperatorAnd
                    ? Expression.AndAlso(left, right)
                    : Expression.OrElse(left, right),
                parameter);
        }
    }
}