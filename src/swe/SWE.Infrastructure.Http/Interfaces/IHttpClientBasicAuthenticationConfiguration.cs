namespace SWE.Infrastructure.Http.Interfaces
{
    public interface IHttpClientBasicAuthenticationConfiguration
        : IHttpClientConfiguration
    {
        string Username { get; }
        string Password { get; }
    }
}