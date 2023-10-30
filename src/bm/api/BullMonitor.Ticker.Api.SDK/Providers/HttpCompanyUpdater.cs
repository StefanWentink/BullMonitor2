using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.SDK.Interfaces;
using SWE.Infrastructure.Http.Interfaces;
using SWE.Infrastructure.Web.Handlers;

namespace BullMonitor.Ticker.Api.SDK.Providers
{
    public class HttpCompanyUpdater
        : BaseHttpClientHandler<IHttpClientConfiguration>
        , IHttpCompanyUpdater
    {
        protected override string HttpClientName => "Company";

        public HttpCompanyUpdater(
            IHttpClientFactory clientFactory,
            IHttpClientConfiguration clientConfiguration)
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

        public async Task<Guid> SetKnownByZacks(
            string code,
            bool value,
            CancellationToken cancellationToken)
        {
            var url = $"/company/setknownbyzacks";
            var request = new CompanySetKnownRequest(code, value);

            return await PutAsync<CompanySetKnownRequest, Guid>(url, request, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Guid> SetKnownByTipRanks(
            string code,
            bool value,
            CancellationToken cancellationToken)
        {
            var url = $"/company/setknownbytipRanks";
            var request = new CompanySetKnownRequest(code, value);

            return await PutAsync<CompanySetKnownRequest, Guid>(url, request, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}