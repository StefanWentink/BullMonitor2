using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Mongo.Interfaces;
using BullMonitor.Data.Mongo.Connectors;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using BullMonitor.Data.Mongo.Interfaces;
using SWE.Mongo;

namespace BullMonitor.Data.Mongo.Extensions
{
    public static class BullMonitorDataMongoServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorMongoServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .WithBullMonitorZacksConnectorServices(configuration)
                .WithBullMonitorTipRanksConnectorServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorZacksConnectorServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection

                .AddSingleton<IZacksConnector, ZacksConnector>()
                .AddSingleton<IUpserter<ZacksEntity>>(x => x.GetRequiredService<IZacksConnector>())
                .AddSingleton<ICreator<ZacksEntity>>(x => x.GetRequiredService<IZacksConnector>())
                .AddSingleton<ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksEntity>>(x => x.GetRequiredService<IZacksConnector>())

                .AddSingleton<IBaseMongoConnector<ZacksEntity>, ZacksMongoConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<ZacksEntity>, ZacksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksEntity>>())
                .AddSingleton<ICollectionAndSingleUpserter<ZacksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksEntity>>())
                .AddSingleton<ICollectionAndSingleUpdater<ZacksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksEntity>>())
                .AddSingleton<ICollectionAndSingleCreator<ZacksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksEntity>>())
                ; 
        }

        internal static IServiceCollection WithBullMonitorTipRanksConnectorServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection

                .AddSingleton<ITipRanksConnector, TipRanksConnector>()
                .AddSingleton<IUpserter<TipRanksEntity>>(x => x.GetRequiredService<ITipRanksConnector>())
                .AddSingleton<ICreator<TipRanksEntity>>(x => x.GetRequiredService<ITipRanksConnector>())
                .AddSingleton<ISingleProvider<(string ticker, DateTimeOffset referenceDate), TipRanksEntity>>(x => x.GetRequiredService<ITipRanksConnector>())

                .AddSingleton<IBaseMongoConnector<TipRanksEntity>, TipRanksMongoConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<TipRanksEntity>, TipRanksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<TipRanksEntity>>())
                .AddSingleton<ICollectionAndSingleUpserter<TipRanksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<TipRanksEntity>>())
                .AddSingleton<ICollectionAndSingleUpdater<TipRanksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<TipRanksEntity>>())
                .AddSingleton<ICollectionAndSingleCreator<TipRanksEntity>>(x => x.GetRequiredService<IBaseMongoConnector<TipRanksEntity>>())
                ; 
        }
    }
}