namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IndustryEntityConfiguration
        : EntityConfiguration<IndustryEntity>
    {
        public override void Configure(EntityTypeBuilder<IndustryEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(IndustryEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .HasIndex(x => x.Code)
                .IsUnique();

            builder
                .Property(x => x.Code)
                .IsRequired();

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.SectorId)
                .IsRequired();

            builder
                .HasOne(x => x.Sector)
                .WithMany(x => x.Industries)
                .HasForeignKey(x => x.SectorId);

            builder
                .HasMany(x => x.Tickers)
                .WithOne(x => x.Industry)
                .HasForeignKey(x => x.IndustryId);
        }
    }
}