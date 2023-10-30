using BullMonitor.Data.Storage.Models;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Data.Mongo.Interfaces
{
    public interface IZacksConnector
        : IUpserter<ZacksEntity>
        , ICreator<ZacksEntity>
        , ISingleProvider<(string ticker, DateTimeOffset referenceDate), ZacksEntity>
    {
        Task<IEnumerable<ZacksEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken);

        public Task<ZacksEntity> CreateIfNotExists(
            ZacksEntity value,
            CancellationToken cancellationToken);
    }
}