namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CurrencyEntityConfiguration
        : EntityCodeConfiguration<CurrencyEntity>
    {
        public override void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(CurrencyEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Ignore(x => x.Tickers);

            builder
                .HasMany(x => x.Tickers)
                .WithOne(x => x.Currency)
                .HasForeignKey(x => x.CurrencyId);
        }
    }
}