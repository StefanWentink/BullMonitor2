using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWE.Infrastructure.Sql.Extensions;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> ApplyConfiguration<TContext>(
            this DbContextOptionsBuilder<TContext> dbContextOptionsBuilder,
            string connectionString,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var maxRetryCount = configuration
                .GetMaxRetryCount();

            var maxRetryDelaySeconds = configuration
                .GetMaxRetryDelaySeconds();

            var commandTimeoutSeconds = configuration
                .GetCommandTimeoutSeconds();

            var minimumConnectionPoolSize = configuration
                .GetMinimumConnectionPoolSize();

            var maximumConnectionPoolSize = configuration
                .GetMaximumConnectionPoolSize();

            var multipleActiveResultSets = configuration
                .GetMultipleActiveResultSets();

            connectionString = connectionString
                .AppendMaxConnectionPoolSize(maximumConnectionPoolSize)
                .AppendMinConnectionPoolSize(minimumConnectionPoolSize)
                .AppendMultipleActiveResultSets(multipleActiveResultSets);

            return dbContextOptionsBuilder
                .SetupBuilder(
                     connectionString,
                     maxRetryCount,
                     maxRetryDelaySeconds,
                     commandTimeoutSeconds);
        }

        public static DbContextOptionsBuilder<TContext> SetupBuilder<TContext>(
            this DbContextOptionsBuilder<TContext> builder,
            string connectionString,
            int maxRetryCount,
            int maxRetryDelaySeconds,
            int commandTimeoutSeconds)
            where TContext : DbContext
        {
            if (maxRetryCount > 0)
            {
                if (maxRetryDelaySeconds > 0)
                {
                    return builder.UseSqlServer(
                        connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: maxRetryCount,
                            maxRetryDelay: TimeSpan.FromSeconds(maxRetryDelaySeconds),
                            errorNumbersToAdd: null);
                            sqlOptions.CommandTimeout(commandTimeoutSeconds);
                        });
                }

                return builder.UseSqlServer(
                    connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: maxRetryCount);
                        sqlOptions.CommandTimeout(commandTimeoutSeconds);
                    });
            }

            return builder.UseSqlServer(connectionString,
                o =>
                {
                    o.CommandTimeout(commandTimeoutSeconds);
                    o.EnableRetryOnFailure();
                });
        }
    }
}
