using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record ExchangeEntity(
            Guid Id,
            string Code,
            string Name
        ) : IIdCode
    {
        public ExchangeEntity(
            string Code,
            string Name)
            : this(
                Guid.Empty,
                Code,
                Name)
        { }

        public virtual ICollection<TickerEntity> Tickers { get; set; } = new HashSet<TickerEntity>();
    }
}