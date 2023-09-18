using BullMonitor.Data.Sql.Configurations;
using BullMonitor.Data.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Sql.Contexts;

namespace BullMonitor.Data.Sql.Contexts
{
    public class TickerContext
        : BaseDbContext
    {
        public TickerContext()
            : this(null)
        { }

        public TickerContext(
            ILogger<TickerContext>? logger)
            : base(logger)
        { }

        public TickerContext(
            DbContextOptions<TickerContext> options,
            ILogger<TickerContext>? logger = null)
            : base(options, logger)
        { }

        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<ExchangeEntity> Exchanges { get; set; }
        public DbSet<IndustryEntity> Industries { get; set; }
        public DbSet<SectorEntity> Sectors { get; set; }
        public DbSet<TickerEntity> Tickers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurations(this);
            modelBuilder.ApplyConfiguration(new CurrencyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ExchangeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new IndustryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SectorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TickerEntityConfiguration());

            foreach (var relation in modelBuilder
                .Model
                .GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}