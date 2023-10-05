using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.SDK.Interfaces
{
    public interface IHttpCompanyProvider
        : ISingleProvider<Guid, CompanyListResponse>
        , ISingleProvider<string, CompanyListResponse>
        , ICollectionProvider<CompanyListResponse>
    { }
}