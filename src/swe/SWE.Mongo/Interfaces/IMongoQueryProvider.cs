using SWE.Mongo.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Mongo.Interfaces
{
    public interface IMongoQueryProvider<T>
        : IProvider<IMongoConditionContainer<T>, T>
    { }
}