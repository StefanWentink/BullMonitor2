using SWE.Infrastructure.Http.Interfaces;
using SWE.Infrastructure.Web.Extensions;
using System.Text;
using System.Text.Json;

namespace SWE.Infrastructure.Web.Handlers
{
    public class BasicAuthenticationHttpClientHandler
        : BaseHttpClientHandler<IHttpClientBasicAuthenticationConfiguration>
    {
        protected BasicAuthenticationHttpClientHandler(
            IHttpClientFactory clientFactory,
            IHttpClientBasicAuthenticationConfiguration clientConfiguration)
            : base(
                  clientFactory,
                  clientConfiguration)
        { }

        protected override JsonSerializerOptions SerializerOptions => new();

        protected override string HttpClientName => string.Empty;
    }

    public class BasicAuthenticationHttpClientHandler<TConfiguration>
        : BaseHttpClientHandler<TConfiguration>
        where TConfiguration : IHttpClientBasicAuthenticationConfiguration
    {
        private static readonly SemaphoreSlim _lock = new(1);

        private string? authorizationHeader = null;

        protected string AuthorizationHeader => authorizationHeader ??= LoadAuthorization();

        protected override JsonSerializerOptions SerializerOptions => new();

        protected override string HttpClientName =>string.Empty;

        public BasicAuthenticationHttpClientHandler(
            IHttpClientFactory clientFactory,
            TConfiguration clientConfiguration)
            : base(
                 clientFactory,
                 clientConfiguration)
        { }

        private string LoadAuthorization()
        {
            try
            {
                _lock.Wait(5000);

                var bytes = Encoding
                    .UTF8
                    .GetBytes($"{ ClientConfiguration.Username }:{ ClientConfiguration.Password }");

                return Convert
                    .ToBase64String(bytes);
            }
            finally
            {
                _lock.Release();
            }
        }

        protected override Func<HttpClient, HttpClient> AuthorizeFunction =>
            (x) =>
            {
                if (x.DefaultRequestHeaders.Authorization != null)
                {
                    return x;
                }

                return x.AddBasicAuthorizationHeader(AuthorizationHeader);
            };
    }
}