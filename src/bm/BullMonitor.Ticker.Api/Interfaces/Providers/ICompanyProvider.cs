using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.Interfaces.Providers
{
    public interface ICompanyProvider
        : ISingleProvider<Guid, CompanyListResponse>
        , ISingleProvider<string, CompanyListResponse>
        , ICollectionProvider<CompanyListResponse>
    { }
}