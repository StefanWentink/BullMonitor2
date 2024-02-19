using BullMonitor.Data.Mongo.Interfaces;
using BullMonitor.Data.Sql.Contexts;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Time.Extensions;
using SWE.Time.Utilities;

namespace BullMonitor.Ticker.Core.Providers
{
    public record ValueProvider(
            IFactory<TickerContext> ContextFactory,
            ICompanyProvider CompanyProvider,
            IZacksConnector ZacksProvider,
            ITipRanksConnector TipRanksProvider,
            IMapper<
                (IEnumerable<ZacksEntity> zacks, IEnumerable<TipRanksEntity> tipranks),
                IDictionary<DateTimeOffset, ValueResponse>> ValueResponseMapper,
            IDateTimeOffsetNow DateTimeOffsetNow,
            ILogger<ValueProvider> Logger)
        : IValueProvider
    {
        private SemaphoreSlim _collectionLock = new(1);

        private IEnumerable<TickerEntity>? _collection = null;


        public async Task<CompanyResponse?> GetSingleOrDefault(
            Guid value,
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .GetSingleOrDefault(value, cancellationToken)
                .ConfigureAwait(false);

            return (await Get(cancellationToken).ConfigureAwait(false))
                .SingleOrDefault(x => x.Id == value);
        }

        public async Task<CompanyResponse?> GetSingleOrDefault(
            string value,
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .GetSingleOrDefault(value, cancellationToken)
                .ConfigureAwait(false);

            return (await Get(tickers.Yield(), cancellationToken).ConfigureAwait(false))
                .SingleOrDefault(x => x.Code.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<CompanyResponse>> Get(
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .Get(cancellationToken)
                .ConfigureAwait(false);

            return await Get(tickers, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByAll(
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .GetKnownByAll(cancellationToken)
                .ConfigureAwait(false);

            return await Get(tickers, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByZacks(
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .GetKnownByZacks(cancellationToken)
                .ConfigureAwait(false);

            return await Get(tickers, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CompanyResponse>> GetKnownByTipRanks(
            CancellationToken cancellationToken)
        {
            var tickers = await CompanyProvider
                .GetKnownByTipRanks(cancellationToken)
                .ConfigureAwait(false);

            return await Get(tickers, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IEnumerable<CompanyResponse>> Get(
            IEnumerable<CompanyListResponse> companies,
            CancellationToken cancellationToken)
        {
            var results = new List<CompanyResponse>();

            try
            {
                var referenceDate = DateTimeOffsetNow
                    .Now
                    .Add(
                        years: -1,
                        timeZoneInfo: TimeZoneInfoUtilities.DutchTimeZoneInfo);

                foreach (var company in companies)
                {
                    var zacksTask = ZacksProvider
                        .GetSince((company.Code, referenceDate), cancellationToken);

                    var tipRanksTask = TipRanksProvider
                        .GetSince((company.Code, referenceDate), cancellationToken);

                    var zacks = await zacksTask.ConfigureAwait(false);

                    var tipRanks = await tipRanksTask.ConfigureAwait(false);

                    var valueDictionary = await ValueResponseMapper
                        .Map(
                            (zacks, tipRanks),
                            cancellationToken)
                        .ConfigureAwait(false);

                    var result = new CompanyResponse(
                        company.Id,
                        company.Code,
                        valueDictionary);

                    results.Add(result);
                }

                return results;
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