using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record struct PriceEntity(
            Guid TickerId,
            DateTimeOffset Date,
            double Value)
        : ITickerValue
    { }
}