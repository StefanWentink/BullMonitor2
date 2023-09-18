namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TickerEntityConfiguration
        : EntityConfiguration<TickerEntity>
    {
        public override void Configure(EntityTypeBuilder<TickerEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(TickerEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .HasIndex(x => x.Code)
                .IsUnique();

            builder
                .Property(x => x.Code)
                .IsRequired();

            builder
                .Property(x => x.IndustryId)
                .IsRequired();

            builder
                .HasOne(x => x.Industry)
                .WithMany(x => x.Tickers)
                .HasForeignKey(x => x.IndustryId);

            builder
                .Property(x => x.CurrencyId)
                .IsRequired();

            builder
                .HasOne(x => x.Currency)
                .WithMany(x => x.Tickers)
                .HasForeignKey(x => x.CurrencyId);

            builder
                .Property(x => x.ExchangeId)
                .IsRequired();

            builder
                .HasOne(x => x.Exchange)
                .WithMany(x => x.Tickers)
                .HasForeignKey(x => x.ExchangeId);

            //builder
            //    .HasMany(x => x.ZacksPriceTargets)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.ZacksRanks)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.ZacksRecommendations)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.TipRanksConsensuses)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.TipRanksPrices)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.TipRanksScores)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);
        }
    }
}