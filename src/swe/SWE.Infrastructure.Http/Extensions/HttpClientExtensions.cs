using Ardalis.GuardClauses;
using SWE.Infrastructure.Http.Constants;

namespace SWE.Infrastructure.Web.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Add Basic authentication to <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="authorizationHeader"></param>
        /// <exception cref="ExceptionWithSubject">When <see cref="HttpHeaderConstants.Authorization"/> header already added.</exception>
        /// <returns></returns>
        public static HttpClient AddBasicAuthorizationHeader(
            this HttpClient client,
            string authorizationHeader)
        {
            Guard.Against.Null(client, nameof(client));
            Guard.Against.NullOrWhiteSpace(authorizationHeader, nameof(authorizationHeader));

            if (client.DefaultRequestHeaders.Contains(HttpHeaderConstants.Authorization))
            {
                throw new System.Exception(
                    $"{nameof(HttpClientExtensions)}.{nameof(AddBasicAuthorizationHeader)}: Header '{HttpHeaderConstants.Authorization}' was already added.");
            }

            client
                .DefaultRequestHeaders
                .Add(
                    HttpHeaderConstants.Authorization,
                    $"{HttpHeaderConstants.Basic} {authorizationHeader}");

            return client;
        }
    }
}