using BullMonitor.Abstractions.Commands;
using BullMonitor.Data.Storage.Interfaces;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Process.Models;
using BullMonitor.Ticker.Process.Utilities;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enums;
using SWE.Rabbit.Abstractions.Interfaces;
using System.Linq.Expressions;

namespace BullMonitor.Ticker.Process.Handlers
{
    public class TickerSyncMessageHandler
        : BaseMessageHandler<TickerSyncMessage>
    {
        private SemaphoreSlim _lock => new(LockSize);
        protected ISqlProvider<ExchangeEntity> ExchangeProvider { get; }
        protected ISqlProvider<CurrencyEntity> CurrencyProvider { get; }
        protected ISqlProvider<SectorEntity> SectorProvider { get; }
        protected ISqlProvider<IndustryEntity> IndustryProvider { get; }
        protected ISqlProvider<TickerEntity> TickerProvider { get; }

        protected ICollectionAndSingleCreator<SectorEntity> SectorCreator { get; }
        protected ICollectionAndSingleCreator<IndustryEntity> IndustryCreator { get; }
        protected ICollectionAndSingleCreator<TickerEntity> TickerCreator { get; }

        public TickerSyncMessageHandler(
            ISqlProvider<ExchangeEntity> exchangeProvider,
            ISqlProvider<CurrencyEntity> currencyProvider,
            ISqlProvider<SectorEntity> sectorProvider,
            ISqlProvider<IndustryEntity> industryProvider,
            ISqlProvider<TickerEntity> tickerProvider,
            ICollectionAndSingleCreator<SectorEntity> sectorCreator,
            ICollectionAndSingleCreator<IndustryEntity> industryCreator,
            ICollectionAndSingleCreator<TickerEntity> tickerCreator,
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<BaseMessageHandler<TickerSyncMessage>> logger)
        :base(
             issueModelSender,
             dateTimeOffsetNow,
             logger)
        {
            ExchangeProvider = exchangeProvider;
            CurrencyProvider = currencyProvider;
            SectorProvider = sectorProvider;
            IndustryProvider = industryProvider;
            TickerProvider = tickerProvider;
            SectorCreator = sectorCreator;
            IndustryCreator = industryCreator;
            TickerCreator = tickerCreator;
        }

        public override async Task<MessageHandlingResponse> Handle(
            TickerSyncMessage value,
            CancellationToken cancellationToken)
        {
            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var exchanges = await ExchangeProvider
                    .Get(new SqlConditionContainer<ExchangeEntity>(), cancellationToken)
                    .ConfigureAwait(false);

                var currency = await CurrencyProvider
                    .GetSingleOrDefault(new SqlConditionContainer<CurrencyEntity>(x => x.Code == "USD"), cancellationToken)
                    .ConfigureAwait(false);

                foreach (var exchange in exchanges)
                {
                    var collection = await CsvUtilities
                        .Read(exchange.Code, cancellationToken)
                        .ConfigureAwait(false);

                    var sectorDictionary = await FileToEntities_1<SectorEntity>(
                           collection,
                           SectorProvider,
                           SectorCreator,
                           x => x.Sector,
                           x => new SectorEntity(x.Sector),
                           cancellationToken)
                        .ConfigureAwait(false);

                    var industryDictionary = await FileToEntities_3(
                           collection,
                           IndustryProvider,
                           IndustryCreator,
                           x => x.Industry,
                           x => x.Sector,
                           sectorDictionary,
                           (FileTickerModel model, Guid parentId) => new IndustryEntity(model.Industry, model.Industry, parentId),
                           cancellationToken)
                        .ConfigureAwait(false);

                    //var companyDictionary = await FileToEntities_4(
                    //       collection,
                    //       CompanyProvider,
                    //       CompanyCreator,
                    //       x => x.Name,
                    //       x => x.Industry,
                    //       industryDictionary,
                    //       (FileTickerModel model, Guid parentId) => new CompanyEntity(model.Name, model.Country, parentId),
                    //       x => x.Name,
                    //       cancellationToken)
                    //    .ConfigureAwait(false);

                    var tickerDictionary = await FileToEntities_3(
                           collection,
                           TickerProvider,
                           TickerCreator,
                           x => x.Ticker,
                           x => x.Industry,
                           industryDictionary,
                           (FileTickerModel model, Guid parentId) => new TickerEntity(model.Ticker, model.Name, parentId, exchange.Id, currency.Id),
                           cancellationToken)
                        .ConfigureAwait(false);
                }

                return MessageHandlingResponse.Successful;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                await SendExceptionIssue(exception, cancellationToken)
                    .ConfigureAwait(false);
                return MessageHandlingResponse.Unsuccessful;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<IDictionary<string, Guid>> FileToEntities_1<TEntity>(
            IEnumerable<FileTickerModel> collection,
            ISqlProvider<TEntity> provider,
            ICollectionAndSingleCreator<TEntity> creator,
            Expression<Func<FileTickerModel, string>> propertySelector,
            Func<FileTickerModel, TEntity> constructor,
            CancellationToken cancellationToken)
            where TEntity : class, IIdCode
        {
            return await FileToEntities_2(
                     collection,
                     provider,
                     creator,
                     propertySelector,
                     x => x.Code,
                     constructor,
                     cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IDictionary<string, Guid>> FileToEntities_2<TEntity>(
            IEnumerable<FileTickerModel> collection,
            ISqlProvider<TEntity> provider,
            ICollectionAndSingleCreator<TEntity> creator,
            Expression<Func<FileTickerModel, string>> propertySelector,
            Expression<Func<TEntity, string>> entityPropertyExpression,
            Func<FileTickerModel, TEntity> constructor,
            CancellationToken cancellationToken)
            where TEntity : class, IIdCode
        {
            var propertyValues = collection
                .Select(x => propertySelector.Compile()(x))
                .Distinct();

            var selector = propertyValues
                .ToExpression(entityPropertyExpression);

            var condition = new SqlConditionContainer<TEntity>(selector);

            var existing = await provider
                .Get(condition, cancellationToken)
                .ConfigureAwait(false);

            var existingValues = existing
                .Select(e => entityPropertyExpression.Compile()(e))
                .ToList();

            var expression = existingValues.ToExpression(propertySelector);

            var creates = collection
                .Where(x => !expression.Compile()(x))
                .ToList();

            if (creates.Count > 0)
            {
                var createEntities = creates
                    .Select(create => constructor(create))
                    .DistinctBy(e => entityPropertyExpression.Compile()(e))
                    .ToList();

                var created = await creator
                    .Create(createEntities, cancellationToken)
                    .ConfigureAwait(false);

                Logger.LogInformation($"Created {created.Count()} {typeof(TEntity).Name}.");
            }
            else
            {
                Logger.LogInformation($"No {typeof(TEntity).Name} to create");
            }

            existing = await provider
                .Get(new SqlConditionContainer<TEntity>(), cancellationToken)
                .ConfigureAwait(false);

            return existing.ToDictionary(x => entityPropertyExpression.Compile()(x), x => x.Id); ;
        }

        private async Task<IDictionary<string, Guid>> FileToEntities_3<TEntity>(
            IEnumerable<FileTickerModel> collection,
            ISqlProvider<TEntity> provider,
            ICollectionAndSingleCreator<TEntity> creator,
            Expression<Func<FileTickerModel, string>> propertySelector,
            Expression<Func<FileTickerModel, string>> propertyParentSelector,
            IDictionary<string, Guid> parentDictionary,
            Func<FileTickerModel, Guid, TEntity> constructor,
            CancellationToken cancellationToken)
            where TEntity : class, IIdCode
        {
            return await FileToEntities_4(
                    collection,
                    provider,
                    creator,
                    propertySelector,
                    propertyParentSelector,
                    parentDictionary,
                    constructor,
                    x => x.Code,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<IDictionary<string, Guid>> FileToEntities_4<TEntity>(
            IEnumerable<FileTickerModel> collection,
            ISqlProvider<TEntity> provider,
            ICollectionAndSingleCreator<TEntity> creator,
            Expression<Func<FileTickerModel, string>> propertySelector,
            Expression<Func<FileTickerModel, string>> propertyParentSelector,
            IDictionary<string, Guid> parentDictionary,
            Func<FileTickerModel, Guid, TEntity> constructor,
            Expression<Func<TEntity, string>> entityPropertyExpression,
            CancellationToken cancellationToken)
            where TEntity : class, IIdCode
        {
            var propertyValues = collection
                .Select(x => propertySelector.Compile()(x))
                .Distinct();

            var selector = propertyValues
                .ToExpression(entityPropertyExpression);

            var condition = new SqlConditionContainer<TEntity>(selector);

            var existing = await provider
                .Get(condition, cancellationToken)
                .ConfigureAwait(false);

            var existingValues = existing
                .Select(e => entityPropertyExpression.Compile()(e))
                .ToList();

            var expression = existingValues.ToExpression(propertySelector);

            var creates = collection
                .Where(x => !expression.Compile()(x))
                .ToList();

            if (creates.Count > 0)
            {
                var createEntities = creates
                    .Select(create =>
                    {
                        var parentValue = propertyParentSelector.Compile()(create);
                        if (!parentDictionary.ContainsKey(parentValue))
                        {
                            throw new KeyNotFoundException($"{nameof(parentDictionary)} does not contain '{parentValue}'");
                        }

                        var parentKey = parentDictionary[parentValue];

                        return constructor(
                            create,
                            parentKey);
                    })
                    .DistinctBy(e => entityPropertyExpression.Compile()(e))
                    .ToList();

                    var created = await creator
                        .Create(createEntities, cancellationToken)
                        .ConfigureAwait(false);

                Logger.LogInformation($"Created {created.Count()} {typeof(TEntity).Name}.");
            }
            else
            {
                Logger.LogInformation($"No {typeof(TEntity).Name} to create");
            }

            existing = await provider
                .Get(new SqlConditionContainer<TEntity>(), cancellationToken)
                .ConfigureAwait(false);

            return existing.ToDictionary(x => entityPropertyExpression.Compile()(x), x => x.Id); ;
        }
    }
}