using MongoDB.Driver;
using SWE.Mongo.Interfaces;
using SWE.Extensions.Extensions;
using System.Linq.Expressions;

namespace SWE.Mongo.Containers
{
    public class MongoConditionContainer<T>
        : MongoConditionContainer<T, T>
        , IMongoConditionContainer<T>
    {
        public MongoConditionContainer()
            : base(
                Enumerable.Empty<FilterDefinition<T>>(),
                x => x)
        { }

        public MongoConditionContainer(
            Expression<Func<T, bool>> expression)
            : base(
                  expression.Yield(),
                  x => x)
        { }

        public MongoConditionContainer(
            IEnumerable<Expression<Func<T, bool>>> expressions)
            : base(
                  expressions,
                  x => x)
        { }

        public MongoConditionContainer(
            FilterDefinition<T> filter)
            : base(
                  filter,
                  x => x)
        { }

        public MongoConditionContainer(
            IEnumerable<FilterDefinition<T>> filters)
            : base(
                  filters,
                  x => x)
        { }
    }

    public class MongoConditionContainer<T, TProjection>
        //: ConditionContainer<T>
        : IMongoConditionContainer<T, TProjection>
    {
        public ICollection<FilterDefinition<T>> Filters { get; }
        public ProjectionDefinition<T, TProjection> Projection { get; }
        public AggregateOptions? AggregateOptions { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 0;

        public MongoConditionContainer(
            Expression<Func<T, TProjection>> projectionExpression)
            : this(
                 Enumerable.Empty<FilterDefinition<T>>(),
                 Builders<T>.Projection.Expression(projectionExpression))
        { }

        public MongoConditionContainer(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, TProjection>> projection)
            : this(
                  expression.Yield(),
                  projection)
        { }

        public MongoConditionContainer(
            IEnumerable<Expression<Func<T, bool>>> expressions,
            Expression<Func<T, TProjection>> projection)
            : this(
                  expressions.Select(x => new ExpressionFilterDefinition<T>(x)),
                  projection)
        { }

        public MongoConditionContainer(
            IEnumerable<FilterDefinition<T>> filters,
            Expression<Func<T, TProjection>> projectionExpression)
            : this(
                 filters,
                 Builders<T>.Projection.Expression(projectionExpression))
        { }

        public MongoConditionContainer(
            FilterDefinition<T> filter,
            Expression<Func<T, TProjection>> projectionExpression)
            : this(
                  filter.Yield(),
                 Builders<T>.Projection.Expression(projectionExpression))
        { }

        public MongoConditionContainer(
            ProjectionDefinition<T, TProjection> projection)
            : this(
                 Enumerable.Empty<FilterDefinition<T>>(),
                 projection)
        { }

        public MongoConditionContainer(
            FilterDefinition<T> filter,
            ProjectionDefinition<T, TProjection> projection)
            : this(
                  filter.Yield(),
                  projection)
        { }

        public MongoConditionContainer(
            IEnumerable<FilterDefinition<T>> filters,
            ProjectionDefinition<T, TProjection> projection)
        {
            Filters = filters.ToList();
            Projection = projection;
        }

        public void AddFilter(FilterDefinition<T> filter)
        {
            Filters.Add(filter);
        }

        public void AddFilter(Expression<Func<T, bool>> expression)
        {
            Filters.Add(new ExpressionFilterDefinition<T>(expression));
        }
    }
}