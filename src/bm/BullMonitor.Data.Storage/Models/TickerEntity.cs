using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record TickerEntity(
            Guid Id,
            string Code,
            string Name,
            Guid IndustryId,
            Guid ExchangeId,
            Guid CurrencyId)
        : IIdCode
    {
        public TickerEntity(
            string Code,
            string Name,
            Guid IndustryId,
            Guid ExchangeId,
            Guid CurrencyId)
            : this(
                Guid.Empty,
                Code,
                Name,
                IndustryId,
                ExchangeId,
                CurrencyId)
        { }

        public virtual IndustryEntity Industry { get; set; }
        public virtual ExchangeEntity Exchange { get; set; }
        public virtual CurrencyEntity Currency { get; set; }
    }
}