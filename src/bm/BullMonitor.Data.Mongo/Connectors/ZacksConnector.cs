using BullMonitor.Data.Mongo.Interfaces;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Mongo.Containers;
using SWE.Mongo.Interfaces;
using SWE.Time.Extensions;
using System.Linq.Expressions;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class ZacksConnector
        : IZacksConnector
    {
        protected IBaseMongoConnector<ZacksEntity> Connector { get; }
        protected ILogger<ZacksConnector> Logger { get; }

        public ZacksConnector(
            IBaseMongoConnector<ZacksEntity> connector,
            ILogger<ZacksConnector> logger)
        {
            Connector = connector;
            Logger = logger;
        }

        public async Task<IEnumerable<ZacksEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<ZacksEntity>(
                    new List<Expression<Func<ZacksEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb >= value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .Get(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ZacksEntity> Upsert(
            ZacksEntity value,
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

        public async Task<ZacksEntity?> GetSingleOrDefault(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<ZacksEntity>(
                    new List<Expression<Func<ZacksEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb == value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .GetSingleOrDefault(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<ZacksEntity> Create(
            ZacksEntity value,
            CancellationToken cancellationToken)
        {
            return CreateIfNotExists(value, cancellationToken);
        }

        public async Task<ZacksEntity> CreateIfNotExists(
            ZacksEntity value,
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

            return new ZacksEntity(string.Empty);
        }
    }
}