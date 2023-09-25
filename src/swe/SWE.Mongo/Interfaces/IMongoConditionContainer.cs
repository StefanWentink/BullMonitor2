using MongoDB.Driver;
using System.Linq.Expressions;

namespace SWE.Mongo.Interfaces
{
    public interface IMongoConditionContainer<T, TProjection>
        //: IConditionContainer<T>
    {
        ICollection<FilterDefinition<T>> Filters { get; }
        ProjectionDefinition<T, TProjection> Projection { get; }
        AggregateOptions? AggregateOptions { get; }
        int Skip { get; }
        int Take { get; }
        void AddFilter(FilterDefinition<T> filter);
        void AddFilter(Expression<Func<T, bool>> expression);
    }

    public interface IMongoConditionContainer<T>
        : IMongoConditionContainer<T, T>
    { }
}