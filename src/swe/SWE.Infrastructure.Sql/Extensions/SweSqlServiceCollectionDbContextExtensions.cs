using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextWithDefaultOptions<TContext>(this IServiceCollection self, IConfiguration config, string migrationSchema)
            where TContext : DbContext
        {
            return AddDbContextWithDefaultOptions<TContext>(self, config, _ => { }, migrationSchema);
        }

        public static IServiceCollection AddDbContextWithDefaultOptions<TContext>(
            this IServiceCollection self, IConfiguration config, Action<DbContextOptionsBuilder> optionsAction, string migrationSchema)
            where TContext : DbContext
        {
            if (!config.GetValue<bool>("ConnectionStrings:InMemory"))
            {
                return self.AddDbContext<TContext>(ob => optionsAction(ob
                    .UseSqlServer(config["ConnectionStrings:DefaultConnection"],
                        ssob => ssob.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                        .EnableRetryOnFailure()
                        .MigrationsHistoryTable("__EFMigrations", migrationSchema)
                        )
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    )
                );
            }
            else
            {
                return self.AddDbContext<TContext>(ob => optionsAction(ob.UseInMemoryDatabase(typeof(TContext) + ":InMemory", b => b.EnableNullChecks(false))));
            }
        }
    }
}