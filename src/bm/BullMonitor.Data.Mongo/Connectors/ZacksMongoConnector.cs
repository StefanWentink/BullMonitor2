using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SWE.Mongo;
using SWE.Mongo.Interfaces;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class ZacksMongoConnector
        : BaseMongoConnector<IMongoConditionContainer<ZacksEntity>, ZacksEntity>
        , IBaseMongoConnector<ZacksEntity>
    {
        public ZacksMongoConnector(
            IConfiguration configuration,
            ILogger<ZacksMongoConnector> logger)
            : base(
                  configuration,
                  logger)
        { }

        protected override string DatabaseName => "BullMonitor";

        protected override CreateCollectionOptions<ZacksEntity> CreateCollectionOptions =>
            new CreateCollectionOptions<ZacksEntity>
            {
                TimeSeriesOptions = new TimeSeriesOptions(
                    nameof(ZacksEntity.ReferenceDate),
                    nameof(ZacksEntity.Ticker),
                    TimeSeriesGranularity.Minutes)
            };

        protected override IEnumerable<CreateIndexModel<ZacksEntity>> GetIndexes()
        {
            var createIndexOptions = new CreateIndexOptions<ZacksEntity>
            {
            };

            var keysDefinition = Builders<ZacksEntity>
                .IndexKeys
                .Ascending(zacksEntity => zacksEntity.Ticker);
            var createIndexOptionsName = $"{nameof(ZacksEntity.Ticker)}";
            yield return new CreateIndexModel<ZacksEntity>(
                keysDefinition,
                new CreateIndexOptions<ZacksEntity>
                {
                    Name = createIndexOptionsName
                });

            keysDefinition = Builders<ZacksEntity>
                .IndexKeys
                .Ascending(zacksEntity => zacksEntity.ReferenceDateDb)
                .Ascending(zacksEntity => zacksEntity.Ticker);
            createIndexOptionsName = $"{nameof(ZacksEntity.ReferenceDate)}_{nameof(ZacksEntity.Ticker)}";
            yield return new CreateIndexModel<ZacksEntity>(
                keysDefinition,
                new CreateIndexOptions<ZacksEntity>
                {
                    Name = createIndexOptionsName
                });
        }


        public override async Task<IEnumerable<ZacksEntity>> Upsert(
            IEnumerable<ZacksEntity> values,
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

        protected override UpdateDefinition<ZacksEntity> GenerateUpdateDefinition(
            ZacksEntity value)
        {
            return Builders<ZacksEntity>
                .Update
                .Set($"{nameof(ZacksEntity.Value)}", value.Value);
        }

        protected override FilterDefinition<ZacksEntity> GenerateUpdateFilter(
            ZacksEntity value)
        {
            if (!string.IsNullOrWhiteSpace(value.Ticker))
            {
                return Builders<ZacksEntity>
                    .Filter
                    .Eq(
                        $"{nameof(ZacksEntity.Ticker)}",
                        value.Ticker);
            }

            throw new NotImplementedException(
                $"{nameof(ZacksMongoConnector)}_{nameof(GenerateUpdateFilter)}");
        }
    }
}