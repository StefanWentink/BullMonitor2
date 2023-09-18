using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Interfaces.Handlers
{
    public interface IDeletedHandler<in T>
        : IHandler<T>
    { }
}