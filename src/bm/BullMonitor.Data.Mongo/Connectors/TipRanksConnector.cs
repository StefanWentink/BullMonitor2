using BullMonitor.Data.Mongo.Interfaces;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Mongo.Containers;
using SWE.Mongo.Interfaces;
using SWE.Time.Extensions;
using System.Linq.Expressions;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class TipRanksConnector
        : ITipRanksConnector
    {
        protected IBaseMongoConnector<TipRanksEntity> Connector { get; }
        protected ILogger<TipRanksConnector> Logger { get; }

        public TipRanksConnector(
            IBaseMongoConnector<TipRanksEntity> connector,
            ILogger<TipRanksConnector> logger)
        {
            Connector = connector;
            Logger = logger;
        }

        public async Task<IEnumerable<TipRanksEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<TipRanksEntity>(
                    new List<Expression<Func<TipRanksEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb >= value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .Get(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TipRanksEntity> Upsert(
            TipRanksEntity value,
            CancellationToken cancellationToken)
        {
            var response = await GetSingleOrDefault(
                    (value.Ticker, value.ReferenceDate),
                    cancellationToken)
                .ConfigureAwait(false);

            return response == null
                ? await Connector
                    .Update(value, cancellationToken)
                    .ConfigureAwait(false)
                : await Connector
                    .Create(value, cancellationToken)
                    .ConfigureAwait(false);
        }

        public async Task<TipRanksEntity?> GetSingleOrDefault(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<TipRanksEntity>(
                    new List<Expression<Func<TipRanksEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb == value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .GetSingleOrDefault(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<TipRanksEntity> Create(
            TipRanksEntity value,
            CancellationToken cancellationToken)
        {
            return CreateIfNotExists(value, cancellationToken);
        }

        public async Task<TipRanksEntity> CreateIfNotExists(
            TipRanksEntity value,
            CancellationToken cancellationToken)
        {
            var response = await GetSingleOrDefault(
                    (value.Ticker, value.ReferenceDate),
                    cancellationToken)
                .ConfigureAwait(false);

            if (response == null)
            {
                return await Connector
                    .Create(value, cancellationToken)
                    .ConfigureAwait(false);
            }

            return new TipRanksEntity(string.Empty);
        }
    }
}