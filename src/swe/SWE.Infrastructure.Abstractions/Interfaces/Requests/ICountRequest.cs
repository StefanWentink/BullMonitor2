namespace SWE.Infrastructure.Abstractions.Interfaces.Requests
{
    public interface ICountRequest
    {
        Guid? CurrentRequestId { get; }
        long? Count { get; set; }
        long? GetCountByRequestId(Guid requestId);
    }
}