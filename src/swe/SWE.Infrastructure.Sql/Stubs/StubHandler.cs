using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;

namespace SWE.Infrastructure.Sql.Stubs
{
    public class StubHandler<T>
        : IHandler<T>
    {
        public Task Handle(
            T value,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class StubCreatedHandler<T>
        : StubHandler<T>
        , ICreatedHandler<T>
    { }

    public class StubUpdatedHandler<T>
        : StubHandler<T>
        , IUpdatedHandler<T>
    { }

    public class StubDeletedHandler<T>
        : StubHandler<T>
        , IDeletedHandler<T>
    { }
}