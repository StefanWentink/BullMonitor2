using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Tests.Stubs
{
    public class CollectSender<T>
        : ISender<T>
    {
        public ICollection<T> Collection { get; } = new HashSet<T>();

        public Task Send(T value, CancellationToken cancellationToken)
        {
            Collection.Add(value);
            return Task.CompletedTask;
        }
    }
}