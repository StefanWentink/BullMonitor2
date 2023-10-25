using BullMonitor.Data.Sql.Contexts;
using Microsoft.Extensions.Configuration;
using SWE.Configuration.Factory;
using SWE.Infrastructure.Sql.Factories;

namespace BullMonitor.Data.Sql.Factories
{
    public class TickerContextFactory
        : DbContextFactory<TickerContext>
    {
        protected override string ConnectionStringConfigurationKey { get; } = "TickerConnectionString";

        /// <summary>
        /// Constructor for Migrations
        /// </summary>
        public TickerContextFactory()
            : base(ConfigurationFactory.Create().Build())
        { }

        public TickerContextFactory(IConfiguration configuration)
            : base(configuration)
        { }

        public override TickerContext CreateDbContext(string[] args)
        {
            return new TickerContext(Options);
        }
    }
}