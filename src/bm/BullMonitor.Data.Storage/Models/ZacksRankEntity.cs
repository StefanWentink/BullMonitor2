using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record struct ZacksRankEntity(
            Guid TickerId,
            DateTimeOffset Date,
            int Rank,
            int Value,
            int Growth,
            int Momentum,
            int VGM)
        : ITickerValue
    { }
}