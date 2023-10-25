using BullMonitor.Data.Mongo.Interfaces;
using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Logging;
using SWE.Mongo.Containers;
using SWE.Mongo.Interfaces;
using SWE.Time.Extensions;
using System.Linq.Expressions;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class ZacksRankConnector
        : IZacksRankConnector
    {
        protected IBaseMongoConnector<ZacksRankEntity> Connector { get; }
        protected ILogger<ZacksRankConnector> Logger { get; }

        public ZacksRankConnector(
            IBaseMongoConnector<ZacksRankEntity> connector,
            ILogger<ZacksRankConnector> logger)
        {
            Connector = connector;
            Logger = logger;
        }

        public async Task<IEnumerable<ZacksRankEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<ZacksRankEntity>(
                    new List<Expression<Func<ZacksRankEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb >= value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .Get(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ZacksRankEntity> Upsert(
            ZacksRankEntity value,
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

        public async Task<ZacksRankEntity?> GetSingleOrDefault(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken)
        {
            var request = new MongoConditionContainer<ZacksRankEntity>(
                    new List<Expression<Func<ZacksRankEntity, bool>>>
                    {
                        x => x.Ticker == value.ticker,
                        x => x.ReferenceDateDb == value.referenceDate.ToUtcTimeZone().DateTime
                    })
            { };

            return await Connector
                .GetSingleOrDefault(request, cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<ZacksRankEntity> Create(
            ZacksRankEntity value,
            CancellationToken cancellationToken)
        {
            return CreateIfNotExists(value, cancellationToken);
        }

        public async Task<ZacksRankEntity> CreateIfNotExists(
            ZacksRankEntity value,
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

            return new ZacksRankEntity(string.Empty);
        }
    }
}