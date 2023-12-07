using BullMonitor.Data.Sql.Contexts;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using System.Linq.Expressions;

namespace BullMonitor.Ticker.Core.Providers
{
    public record CompanyProvider(
            IFactory<TickerContext> ContextFactory,
            IMapper<TickerEntity, CompanyListResponse> Mapper,
            ISqlProvider<TickerEntity> SqlProvider,
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

        public Task<IEnumerable<CompanyListResponse>> Get(
            CancellationToken cancellationToken)
        {
            return Get(null, cancellationToken);
        }

        public Task<IEnumerable<CompanyListResponse>> GetKnownByAll(
            CancellationToken cancellationToken)
        {
            return Get(x => (x.KnownByZacks ?? false) && (x.KnownByTipRanks ?? false), cancellationToken);
        }

        public Task<IEnumerable<CompanyListResponse>> GetKnownByZacks(
            CancellationToken cancellationToken)
        {
            return Get(x => (x.KnownByZacks ?? true), cancellationToken);
        }

        public Task<IEnumerable<CompanyListResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken)
        {
            return Get(x => x.KnownByTipRanks == null || x.KnownByTipRanks == true, cancellationToken);
        }

        private async Task<IEnumerable<CompanyListResponse>> Get(
            Expression<Func<TickerEntity, bool>>? expression,
            CancellationToken cancellationToken)
        {
            try
            {
                await _collectionLock.WaitAsync(cancellationToken).ConfigureAwait(false);

                var container = expression is null
                    ? new SqlConditionContainer<TickerEntity>()
                    : new SqlConditionContainer<TickerEntity>(expression);

                var tickers = await SqlProvider
                    .Get(container, cancellationToken)
                    .ConfigureAwait(false);

                var tasks = tickers
                        .Select(x => Mapper.Map(x, cancellationToken));

                    _collection = await Task.WhenAll(tasks).ConfigureAwait(false);

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