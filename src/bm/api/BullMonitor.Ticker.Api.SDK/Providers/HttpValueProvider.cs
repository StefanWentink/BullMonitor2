using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.SDK.Interfaces;
using SWE.Infrastructure.Http.Interfaces;
using SWE.Infrastructure.Web.Handlers;

namespace BullMonitor.Ticker.Api.SDK.Providers
{
    public class HttpValueProvider
        : BaseHttpClientHandler<IHttpClientConfiguration>
        , IHttpValueProvider
    {
        protected override string HttpClientName => "Value";

        public HttpValueProvider(
            IHttpClientFactory clientFactory,
            IHttpClientConfiguration clientConfiguration)
            : base(
                  clientFactory,
                  clientConfiguration)
        { }

        public async Task<IEnumerable<CompanyResponse>> Get(
            CancellationToken cancellationToken)
        {
            var url = $"/value/get";

            return await GetAsync<IEnumerable<CompanyResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByAll(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbyall";

            return await GetAsync<IEnumerable<CompanyResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByZacks(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbyzacks";

            return await GetAsync<IEnumerable<CompanyResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken)
        {
            var url = $"/value/getknownbytipRanks";

            return await GetAsync<IEnumerable<CompanyResponse>>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CompanyResponse?> GetSingleOrDefault(
            Guid value,
            CancellationToken cancellationToken)
        {
            var url = $"/value/getbyid/{value}";

            return await GetAsync<CompanyResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<CompanyResponse?> GetSingleOrDefault(
            string value,
            CancellationToken cancellationToken)
        {
            var url = $"/value/getbycode/{value}";

            return await GetAsync<CompanyResponse>(url, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}