using SWE.Infrastructure.Abstractions.Factories;
using SWE.Infrastructure.Http.Interfaces;
using SWE.Infrastructure.Web.Exceptions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SWE.Infrastructure.Web.Handlers
{
    public abstract class BaseHttpClientHandler<TConfiguration>
        : IBaseHttpClientHandler
        where TConfiguration : IHttpClientConfiguration
    {
        protected IHttpClientFactory ClientFactory { get; }

        protected TConfiguration ClientConfiguration { get; }

        protected virtual JsonSerializerOptions SerializerOptions => JsonSerializerOptionsFactory.Options;

        protected abstract string HttpClientName { get; }

        protected virtual ICollection<HttpStatusCode> GetAcceptedStatusCodes { get; }
            = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.Accepted,
                HttpStatusCode.NoContent,
            };

        protected virtual ICollection<HttpStatusCode> PutAcceptedStatusCodes { get; }
            = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.Accepted,
                HttpStatusCode.Created
            };

        protected virtual ICollection<HttpStatusCode> PostAcceptedStatusCodes { get; }
            = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.Accepted,
                HttpStatusCode.Created
            };

        protected virtual ICollection<HttpStatusCode> DeleteAcceptedStatusCodes { get; }
            = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.OK,
                HttpStatusCode.Accepted,
                HttpStatusCode.NoContent
            };

        /// <summary>
        /// Override this to get authorization headers in place
        /// </summary>
        protected virtual Func<HttpClient, HttpClient> AuthorizeFunction { get; } =
            (x) => x;

        protected BaseHttpClientHandler(
            IHttpClientFactory clientFactory,
            TConfiguration clientConfiguration)
        {
            ClientFactory = clientFactory;
            ClientConfiguration = clientConfiguration;
        }

        public async Task<TResponse?> GetAsync<TResponse>(
            string url,
            CancellationToken cancellationToken)
            where TResponse : class
        {
            var uri = CombineUriWithConfiguration(url);

            var client = ClientFactory.CreateClient(HttpClientName);

            var response = await AuthorizeFunction(client)
                .GetAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            return await HandleResponse<TResponse>(
                    response,
                    GetAcceptedStatusCodes,
                    url,
                    HttpMethod.Get,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(
            string url,
            CancellationToken cancellationToken)
        {
            var uri = CombineUriWithConfiguration(url);

            var client = ClientFactory.CreateClient(HttpClientName);

            var response = await AuthorizeFunction(client)
                .DeleteAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            return HandleResponse(
                    response,
                    DeleteAcceptedStatusCodes,
                    url,
                    HttpMethod.Delete);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken)
        {
            var uri = CombineUriWithConfiguration(url);

            var content = JsonSerializer.Serialize(request, SerializerOptions);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient(HttpClientName);

            var response = await AuthorizeFunction(client)
                .PostAsync(uri, stringContent, cancellationToken)
                .ConfigureAwait(false);

            return await HandleResponse<TResponse>(
                    response,
                    PostAcceptedStatusCodes,
                    url,
                    HttpMethod.Post,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            CancellationToken cancellationToken)
        {
            var uri = CombineUriWithConfiguration(url);

            var content = JsonSerializer.Serialize(request, SerializerOptions);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient(HttpClientName);

            var response = await AuthorizeFunction(client)
                .PutAsync(uri, stringContent, cancellationToken)
                .ConfigureAwait(false);

            return await HandleResponse<TResponse>(
                    response,
                    PutAcceptedStatusCodes,
                    url,
                    HttpMethod.Put,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private string CombineUriWithConfiguration(string url)
        {
            var uri = url.Replace("http:", "https:");

            return uri.Contains(ClientConfiguration.Uri)
                ? uri
                : ClientConfiguration.Uri + uri;
        }

        private async Task<TResponse?> HandleResponse<TResponse>(
            HttpResponseMessage response,
            IEnumerable<HttpStatusCode> acceptedStatusCodes,
            string url,
            HttpMethod verb,
            CancellationToken cancellationToken)
        {
            if (acceptedStatusCodes.Contains(response.StatusCode))
            {
                var stringContent = await response
                    .Content
                    .ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                return JsonSerializer.Deserialize<TResponse>(stringContent, SerializerOptions);
            }

            throw new HttpException(
                url,
                verb,
                response.StatusCode,
                response.ReasonPhrase);
        }

        private static bool HandleResponse(
            HttpResponseMessage response,
            IEnumerable<HttpStatusCode> acceptedStatusCodes,
            string url,
            HttpMethod verb)
        {
            if (acceptedStatusCodes.Contains(response.StatusCode))
            {
                return true;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                case HttpStatusCode.OK:
                    return false;

                default:
                    throw new HttpException(
                        url,
                        verb,
                        response.StatusCode,
                        response.ReasonPhrase);
            }
        }
    }
}