using BullMonitor.Data.Storage.Models;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Data.Mongo.Interfaces
{
    public interface ITipRanksConnector
        : IUpserter<TipRanksEntity>
        , ICreator<TipRanksEntity>
        , ISingleProvider<(string ticker, DateTimeOffset referenceDate), TipRanksEntity>
    {
        Task<IEnumerable<TipRanksEntity>> GetSince(
            (string ticker, DateTimeOffset referenceDate) value,
            CancellationToken cancellationToken);

        public Task<TipRanksEntity> CreateIfNotExists(
            TipRanksEntity value,
            CancellationToken cancellationToken);
    }
}