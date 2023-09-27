using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.SDK.Interfaces;
using SWE.Infrastructure.Http.Configurations;
using SWE.Infrastructure.Web.Handlers;
using System.Text.Json;

namespace BullMonitor.Ticker.Api.SDK.Providers
{
    public class HttpCompanyProvider
        : BaseHttpClientHandler<HttpClientConfiguration>
        , IHttpCompanyProvider
    {
        protected override JsonSerializerOptions SerializerOptions => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        protected override string HttpClientName => "Company";

        public HttpCompanyProvider(
            IHttpClientFactory clientFactory,
            HttpClientConfiguration clientConfiguration)
            : base(
                  clientFactory,
                  clientConfiguration)
        { }

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            Guid value,
            CancellationToken cancellationToken)
        {
            var url = $"/company/getbyid/{value}";

            return await GetAsync<CompanyListResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            string value,
            CancellationToken cancellationToken)
        {
            var url = $"/company/getbycode/{value}";

            return await GetAsync<CompanyListResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyListResponse>> Get(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}