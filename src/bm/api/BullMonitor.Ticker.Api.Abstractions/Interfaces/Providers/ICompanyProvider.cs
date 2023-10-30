using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers
{
    public interface ICompanyProvider
        : ISingleProvider<Guid, CompanyListResponse>
        , ISingleProvider<string, CompanyListResponse>
        , ICollectionProvider<CompanyListResponse>
    {
        Task<IEnumerable<CompanyListResponse>> GetKnownByZacks(
                CancellationToken cancellationToken);

        Task<IEnumerable<CompanyListResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken);
    }
}