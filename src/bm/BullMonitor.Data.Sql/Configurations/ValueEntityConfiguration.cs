namespace BullMonitor.Data.Sql.Configurations
{
    using BullMonitor.Data.Storage.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ValueEntityConfiguration
        : EntityConfiguration<ValueEntity>
    {
        public override void Configure(EntityTypeBuilder<ValueEntity> builder)
        {
            base.Configure(builder);

            builder
                .ToTable(typeof(ValueEntity).Name.Replace("Entity", string.Empty, StringComparison.OrdinalIgnoreCase));

            builder
                .HasOne(x => x.Ticker)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.ZacksPriceTargets)
            //    .WithOne(x => x.Ticker)
            //    .HasForeignKey(x => x.TickerId);

            //builder
            //    .HasMany(x => x.Zacks)
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