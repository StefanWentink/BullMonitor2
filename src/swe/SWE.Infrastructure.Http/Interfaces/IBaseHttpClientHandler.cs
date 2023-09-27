namespace SWE.Infrastructure.Http.Interfaces
{
    public interface IBaseHttpClientHandler
    {
        Task<bool> DeleteAsync(
            string url,
            CancellationToken cancellationToken);

        Task<TResponse?> GetAsync<TResponse>(
            string url,
            CancellationToken cancellationToken)
            where TResponse : class;

        Task<TResponse?> PostAsync<TRequest, TResponse>(
            string url, TRequest request,
            CancellationToken cancellationToken);

        Task<TResponse?> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken);
    }
}