using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SWE.Mongo;
using SWE.Mongo.Interfaces;

namespace BullMonitor.Data.Mongo.Connectors
{
    public class TipRanksMongoConnector
        : BaseMongoConnector<IMongoConditionContainer<TipRanksEntity>, TipRanksEntity>
        , IBaseMongoConnector<TipRanksEntity>
    {
        public TipRanksMongoConnector(
            IConfiguration configuration,
            ILogger<TipRanksMongoConnector> logger)
            : base(
                  configuration,
                  logger)
        { }

        protected override string DatabaseName => "BullMonitor";

        protected override CreateCollectionOptions<TipRanksEntity> CreateCollectionOptions =>
            new CreateCollectionOptions<TipRanksEntity>
            {
                TimeSeriesOptions = new TimeSeriesOptions(
                    nameof(TipRanksEntity.ReferenceDate),
                    nameof(TipRanksEntity.Ticker),
                    TimeSeriesGranularity.Minutes)
            };

        protected override IEnumerable<CreateIndexModel<TipRanksEntity>> GetIndexes()
        {
            var createIndexOptions = new CreateIndexOptions<TipRanksEntity>
            {
            };

            var keysDefinition = Builders<TipRanksEntity>
                .IndexKeys
                .Ascending(zacksEntity => zacksEntity.Ticker);
            var createIndexOptionsName = $"{nameof(TipRanksEntity.Ticker)}";
            yield return new CreateIndexModel<TipRanksEntity>(
                keysDefinition,
                new CreateIndexOptions<TipRanksEntity>
                {
                    Name = createIndexOptionsName
                });

            keysDefinition = Builders<TipRanksEntity>
                .IndexKeys
                .Ascending(zacksEntity => zacksEntity.ReferenceDateDb)
                .Ascending(zacksEntity => zacksEntity.Ticker);
            createIndexOptionsName = $"{nameof(TipRanksEntity.ReferenceDate)}_{nameof(TipRanksEntity.Ticker)}";
            yield return new CreateIndexModel<TipRanksEntity>(
                keysDefinition,
                new CreateIndexOptions<TipRanksEntity>
                {
                    Name = createIndexOptionsName
                });
        }


        public override async Task<IEnumerable<TipRanksEntity>> Upsert(
            IEnumerable<TipRanksEntity> values,
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

        protected override UpdateDefinition<TipRanksEntity> GenerateUpdateDefinition(
            TipRanksEntity value)
        {
            return Builders<TipRanksEntity>
                .Update
                .Set($"{nameof(TipRanksEntity.Value)}", value.Value);
        }

        protected override FilterDefinition<TipRanksEntity> GenerateUpdateFilter(
            TipRanksEntity value)
        {
            if (!string.IsNullOrWhiteSpace(value.Ticker))
            {
                return Builders<TipRanksEntity>
                    .Filter
                    .Eq(
                        $"{nameof(TipRanksEntity.Ticker)}",
                        value.Ticker);
            }

            throw new NotImplementedException(
                $"{nameof(TipRanksMongoConnector)}_{nameof(GenerateUpdateFilter)}");
        }
    }
}