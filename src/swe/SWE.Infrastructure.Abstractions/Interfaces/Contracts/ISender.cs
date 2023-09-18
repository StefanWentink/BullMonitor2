namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface ISender<in T>
    {
        Task Send(T value, CancellationToken cancellationToken);
    }
}