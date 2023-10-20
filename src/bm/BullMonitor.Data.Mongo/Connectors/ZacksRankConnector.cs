using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SWE.Mongo;
using SWE.Mongo.Interfaces;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class ZacksRankConnector
        : BaseMongoConnector<IMongoConditionContainer<ZacksRankEntity>, ZacksRankEntity>
        , IBaseMongoConnector<ZacksRankEntity>
    {
        public ZacksRankConnector(
            IConfiguration configuration,
            ILogger<ZacksRankConnector> logger)
            : base(
                  configuration,
                  logger)
        { }

        protected override string DatabaseName => "BullMonitor";

        protected override CreateCollectionOptions<ZacksRankEntity> CreateCollectionOptions =>
            new CreateCollectionOptions<ZacksRankEntity>
            {
                TimeSeriesOptions = new TimeSeriesOptions(
                    nameof(ZacksRankEntity.ReferenceDate),
                    nameof(ZacksRankEntity.Ticker),
                    TimeSeriesGranularity.Minutes)
            };

        protected override IEnumerable<CreateIndexModel<ZacksRankEntity>> GetIndexes()
        {
            var createIndexOptions = new CreateIndexOptions<ZacksRankEntity>
            {
            };

            var keysDefinition = Builders<ZacksRankEntity>
                .IndexKeys
                .Ascending(zacksRankEntity => zacksRankEntity.Ticker);
            var createIndexOptionsName = $"{nameof(ZacksRankEntity.Ticker)}";
            yield return new CreateIndexModel<ZacksRankEntity>(
                keysDefinition,
                new CreateIndexOptions<ZacksRankEntity>
                {
                    Name = createIndexOptionsName
                });

            keysDefinition = Builders<ZacksRankEntity>
                .IndexKeys
                .Ascending(zacksRankEntity => zacksRankEntity.ReferenceDateDb)
                .Ascending(zacksRankEntity => zacksRankEntity.Ticker);
            createIndexOptionsName = $"{nameof(ZacksRankEntity.ReferenceDate)}_{nameof(ZacksRankEntity.Ticker)}";
            yield return new CreateIndexModel<ZacksRankEntity>(
                keysDefinition,
                new CreateIndexOptions<ZacksRankEntity>
                {
                    Name = createIndexOptionsName
                });
        }


        public override async Task<IEnumerable<ZacksRankEntity>> Upsert(
            IEnumerable<ZacksRankEntity> values,
            CancellationToken cancellationToken)
        {
            var updates = values
                .Select(value =>
                (
                    value,
                    filter: GenerateUpdateFilter(value),
                    update: GenerateUpdateDefinition(value)
                ))
                .ToList();

            foreach (var update in updates)
            {
                await Collection
                        .UpdateOneAsync(
                            update.filter,
                            update.update,
                            new UpdateOptions { IsUpsert = true },
                            cancellationToken)
                        .ConfigureAwait(false);
            }

            return values;
        }

        protected override UpdateDefinition<ZacksRankEntity> GenerateUpdateDefinition(
            ZacksRankEntity value)
        {
            return Builders<ZacksRankEntity>
                .Update
                .Set($"{nameof(ZacksRankEntity.Value)}", value.Value);
        }

        protected override FilterDefinition<ZacksRankEntity> GenerateUpdateFilter(
            ZacksRankEntity value)
        {
            if (!string.IsNullOrWhiteSpace(value.Ticker))
            {
                return Builders<ZacksRankEntity>
                    .Filter
                    .Eq(
                        $"{nameof(ZacksRankEntity.Ticker)}",
                        value.Ticker);
            }

            throw new NotImplementedException(
                $"{nameof(ZacksRankConnector)}_{nameof(GenerateUpdateFilter)}");
        }
    }
}