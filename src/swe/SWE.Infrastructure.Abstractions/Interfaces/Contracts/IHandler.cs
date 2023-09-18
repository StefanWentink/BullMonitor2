namespace SWE.Infrastructure.Abstractions.Interfaces.Contracts
{
    public interface IHandler<in T>
    {
        Task Handle(T value, CancellationToken cancellationToken);
    }
}