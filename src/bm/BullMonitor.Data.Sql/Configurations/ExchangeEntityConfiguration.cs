namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExchangeEntityConfiguration
        : EntityCodeConfiguration<ExchangeEntity>
    {
        public override void Configure(EntityTypeBuilder<ExchangeEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(ExchangeEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Ignore(x => x.Tickers);

            builder
                .HasMany(x => x.Tickers)
                .WithOne(x => x.Exchange)
                .HasForeignKey(x => x.ExchangeId);
        }
    }
}