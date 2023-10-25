using BullMonitor.Data.Storage.Models;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Data.Mongo.Interfaces
{
    public interface IZacksRankConnector
        : IUpserter<ZacksRankEntity>
        , ICreator<ZacksRankEntity>
        , ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksRankEntity>
    {
        Task<IEnumerable<ZacksRankEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken);

        public Task<ZacksRankEntity> CreateIfNotExists(
            ZacksRankEntity value,
            CancellationToken cancellationToken);
    }
}