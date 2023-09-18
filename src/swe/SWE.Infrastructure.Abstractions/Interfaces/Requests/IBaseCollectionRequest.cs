namespace SWE.Infrastructure.Abstractions.Interfaces.Requests
{
    public interface IBaseCollectionRequest
    {
        int? Take { get; set; }
        int? Skip { get; set; }
    }
}