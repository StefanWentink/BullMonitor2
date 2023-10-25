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
                .WithBullMonitorZacksRankConnectorServices(configuration)
                ;
        }

        internal static IServiceCollection WithBullMonitorZacksRankConnectorServices(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection

                .AddSingleton<IZacksRankConnector, ZacksRankConnector>()
                .AddSingleton<IUpserter<ZacksRankEntity>>(x => x.GetRequiredService<IZacksRankConnector>())
                .AddSingleton<ICreator<ZacksRankEntity>>(x => x.GetRequiredService<IZacksRankConnector>())
                .AddSingleton<ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksRankEntity>>(x => x.GetRequiredService<IZacksRankConnector>())

                .AddSingleton<IBaseMongoConnector<ZacksRankEntity>, ZacksRankMongoConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<ZacksRankEntity>, ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleUpserter<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleUpdater<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleCreator<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                ; 
        }
    }
}