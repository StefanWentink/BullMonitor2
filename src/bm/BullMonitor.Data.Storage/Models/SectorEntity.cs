using BullMonitor.Data.Storage.Interfaces;

namespace BullMonitor.Data.Storage.Models
{
    public record SectorEntity(
            Guid Id,
            string Code,
            string Name
        ) : IIdCode
    {
        public SectorEntity(
            string Code,
            string Name)
            : this(
                Guid.Empty,
                Code,
                Name)
        { }
        public SectorEntity(
            string Code)
            : this(
                Code,
                Code)
        { }

        public virtual ICollection<IndustryEntity> Industries { get; set; } = new HashSet<IndustryEntity>();
    }
}