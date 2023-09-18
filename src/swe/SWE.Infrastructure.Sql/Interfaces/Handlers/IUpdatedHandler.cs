using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Interfaces.Handlers
{
    public interface IUpdatedHandler<in T>
        : IHandler<T>
    { }
}