using SWE.Infrastructure.Abstractions.Interfaces.Requests;

namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface IDataProvider<in TCondition, T>
        : IProvider<TCondition, T>
        where TCondition : IConditionContainer<T>
    { }
}