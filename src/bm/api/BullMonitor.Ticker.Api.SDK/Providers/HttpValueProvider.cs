using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.SDK.Interfaces;
using SWE.Infrastructure.Http.Interfaces;
using SWE.Infrastructure.Web.Handlers;

namespace BullMonitor.Ticker.Api.SDK.Providers
{
    public class HttpValueProvider
        : BaseHttpClientHandler<IHttpClientConfiguration>
        , IHttpCompanyProvider
    {
        protected override string HttpClientName => "Value";

        public HttpValueProvider(
            IHttpClientFactory clientFactory,
            IHttpClientConfiguration clientConfiguration)
            : base(
                  clientFactory,
                  clientConfiguration)
        { }

        public async Task<IEnumerable<CompanyListResponse>> Get(
            CancellationToken cancellationToken)
        {
            var url = $"/value/get";

            return await GetAsync<IEnumerable<CompanyListResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyListResponse>> GetKnownByAll(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbyall";

            return await GetAsync<IEnumerable<CompanyListResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyListResponse>> GetKnownByZacks(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbyzacks";

            return await GetAsync<IEnumerable<CompanyListResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyListResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbytipRanks";

            return await GetAsync<IEnumerable<CompanyListResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            Guid value,
            CancellationToken cancellationToken)
        {
            var url = $"/value/getbyid/{value}";

            return await GetAsync<CompanyListResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            string value,
            CancellationToken cancellationToken)
        {
            var url = $"/value/getbycode/{value}";

            return await GetAsync<CompanyListResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}