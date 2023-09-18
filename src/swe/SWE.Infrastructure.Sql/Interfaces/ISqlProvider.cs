using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Interfaces
{
    public interface ISqlProvider<T>
        : IDataProvider<ISqlConditionContainer<T>, T>
    { }
}