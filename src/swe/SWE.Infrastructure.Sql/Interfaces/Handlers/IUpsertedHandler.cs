using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Infrastructure.Sql.Interfaces.Handlers
{
    public interface IUpsertedHandler<in T>
        : IHandler<T>
    { }
}