namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SectorEntityConfiguration
        : EntityCodeConfiguration<SectorEntity>
    {
        public override void Configure(EntityTypeBuilder<SectorEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(SectorEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .HasMany(x => x.Industries)
                .WithOne(x => x.Sector)
                .HasForeignKey(x => x.SectorId);
        }
    }
}