using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record struct PriceEntity(
            Guid TickerId,
            DateTimeOffset ReferenceDate,
            double Value)
        : ITickerValue
    { }
}