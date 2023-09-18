namespace SWE.Infrastructure.Abstractions.Interfaces.Data
{
    public interface IId
        : IId<Guid>
    { }

    public interface IId<TId>
        where TId : IEquatable<TId>
    {
        TId Id { get; set; }
    }
}