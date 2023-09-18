using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using System.Linq.Expressions;
using static MoreLinq.Extensions.ForEachExtension;
using SWE.Extensions.Extensions;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class SqlConditionContainerExtensions
    {
        public static void AppendFilter<T, T2>(
            this ISqlConditionContainer<T> sourceContainer,
            Expression<Func<T, IEnumerable<T2>>> includeExpression,
            ISqlConditionContainer<T2>? filterContainer = null)
            where T : class
            where T2 : class
        {
            var filterCondition = filterContainer?.Expressions.And();

            if (filterCondition != null)
            {
                var anyExpression = includeExpression.CombineFilter(filterCondition);
                sourceContainer.AddExpression(anyExpression);
            }
        }

        public static void AppendFilter<T, T2>(
            this ISqlConditionContainer<T> sourceContainer,
            Expression<Func<T, T2>> includeFunction,
            ISqlConditionContainer<T2>? includeContainer = null)
            where T : class
        {
            var filterExpression = includeContainer?.Expressions.And();

            if (filterExpression != null)
            {
                sourceContainer
                    .AddExpression(
                        includeFunction.CombineSelectorParamExpression(filterExpression));
            }
        }

        public static void AppendInclude<T, T2>(
            this ISqlConditionContainer<T> sourceContainer,
            Expression<Func<T, IEnumerable<T2>>> includeExpression,
            ISqlConditionContainer<T2>? includeContainer)
            where T : class
            where T2 : class
        {
            var includeFilter = includeContainer?.Expressions.And();

            if (includeFilter != null)
            {
                includeExpression = includeExpression.CombineInclude(includeFilter);
            }

            if (includeContainer?.Includes.Any() == true)
            {
                includeContainer?
                    .Includes
                    .ForEach(include => sourceContainer
                        .AddInclude(new IncludeResolver<T, T2>(
                            includeExpression,
                            include)));
            }
            else
            {
                sourceContainer.AddInclude(includeExpression);
            }
        }

        public static void AppendIncludeSingle<T, T2>(
            this ISqlConditionContainer<T> sourceContainer,
            Expression<Func<T, T2>> includeExpression,
            ISqlConditionContainer<T2>? includeContainer = null)
            where T : class
        {
            var includeFilter = includeContainer?.Expressions.And();

            if (includeFilter != null)
            {
                sourceContainer
                    .AddExpression(
                        includeExpression.CombineSelectorParamExpression(includeFilter));
            }

            if (includeContainer?.Includes.Any() == true)
            {
                includeContainer?
                    .Includes
                    .ForEach(include => sourceContainer
                        .AddInclude(new IncludeResolver<T, T2>(
                            includeExpression,
                            include)));
            }
            else
            {
                sourceContainer.AddInclude(includeExpression);
            }
        }
    }
}