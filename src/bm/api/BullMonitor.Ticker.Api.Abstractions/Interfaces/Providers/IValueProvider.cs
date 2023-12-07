using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers
{
    public interface IValueProvider
        : ISingleProvider<Guid, CompanyResponse>
        , ISingleProvider<string, CompanyResponse>
        , ICollectionProvider<CompanyResponse>
    {
        Task<IEnumerable<CompanyResponse>> GetKnownByAll(
                CancellationToken cancellationToken);

        Task<IEnumerable<CompanyResponse>> GetKnownByZacks(
                CancellationToken cancellationToken);

        Task<IEnumerable<CompanyResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken);
    }
}