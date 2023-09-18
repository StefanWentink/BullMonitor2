using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record IndustryEntity(
            Guid Id,
            string Code,
            string Name,
            Guid SectorId
        ) : IIdCode
    {
        public IndustryEntity(
            string Code,
            string Name,
            Guid SectorId)
            : this(
                Guid.Empty,
                Code,
                Name,
                SectorId)
        { }

        public virtual SectorEntity Sector { get; set; }
        public virtual ICollection<TickerEntity> Tickers { get; set; } = new HashSet<TickerEntity>();
    }
}