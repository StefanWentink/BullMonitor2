using BullMonitor.Data.Sql.Contexts;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using BullMonitor.Ticker.Api.Interfaces.Providers;
using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Api.Providers
{
    public record CompanyProvider(
            IFactory<TickerContext> ContextFactory,
            IMapper<TickerEntity, CompanyListResponse> Mapper,
            ILogger<CompanyProvider> Logger)
        : ICompanyProvider
    {
        private SemaphoreSlim _collectionLock = new SemaphoreSlim(1);

        private IEnumerable<CompanyListResponse>? _collection = null;

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            Guid value,
            CancellationToken cancellationToken)
        {
            return (await Get(cancellationToken).ConfigureAwait(false))
                .SingleOrDefault(x => x.Id == value);
        }

        public async Task<CompanyListResponse?> GetSingleOrDefault(
            string value,
            CancellationToken cancellationToken)
        {
            return (await Get(cancellationToken).ConfigureAwait(false))
                .SingleOrDefault(x => x.Code.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<CompanyListResponse>> Get(
            CancellationToken cancellationToken)
        {
            try
            {
                await _collectionLock.WaitAsync(cancellationToken).ConfigureAwait(false);

                using (var context = ContextFactory.Create())
                {
                    var tickers = await context
                        .Tickers
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var tasks = tickers
                        .Select(x => Mapper.Map(x, cancellationToken));

                    _collection = await Task.WhenAll(tasks).ConfigureAwait(false);
                }

                return _collection;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                throw;
            }
            finally
            {
                _collectionLock.Release();
            }
        }
    }
}