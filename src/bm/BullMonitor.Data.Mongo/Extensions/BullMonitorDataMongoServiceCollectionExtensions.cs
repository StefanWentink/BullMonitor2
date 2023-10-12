using BullMonitor.Data.Storage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Mongo.Interfaces;
using BullMonitor.Data.Mongo.Connectors;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Data.Mongo.Extensions
{
    public static class BullMonitorDataMongoServiceCollectionExtensions
    {
        public static IServiceCollection WithBullMonitorTickerValueProcessServices(
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

                .AddSingleton<IBaseMongoConnector<ZacksRankEntity>, ZacksRankConnector>()
                .AddSingleton<IProvider<IMongoConditionContainer<ZacksRankEntity>, ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleUpserter<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleUpdater<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                .AddSingleton<ICollectionAndSingleCreator<ZacksRankEntity>>(x => x.GetRequiredService<IBaseMongoConnector<ZacksRankEntity>>())
                ; 
        }
    }
}