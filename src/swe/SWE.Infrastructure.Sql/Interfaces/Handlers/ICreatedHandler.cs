using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Interfaces.Handlers
{
    public interface ICreatedHandler<in T>
        : IHandler<T>
    { }
}