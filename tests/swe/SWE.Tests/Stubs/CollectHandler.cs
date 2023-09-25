using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Tests.Stubs
{
    public class CollectHandler<T>
        : IHandler<T>
    {
        public Guid Key { get; } = Guid.NewGuid();

        public ICollection<T> Collection { get; } = new HashSet<T>();

        public Task Handle(T value, CancellationToken cancellationToken)
        {
            Collection.Add(value);
            return Task.CompletedTask;
        }
    }
}