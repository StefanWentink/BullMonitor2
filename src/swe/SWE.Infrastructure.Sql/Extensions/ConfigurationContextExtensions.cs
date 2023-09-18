using Microsoft.Extensions.Configuration;
using SWE.Configuration.Extensions;
using SWE.Infrastructure.Sql.Constants;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class ConfigurationContextExtensions
    {
        public static int GetCommandTimeoutSeconds(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsIntValue(
                    ConnectionStringConstants.CommandTimeoutSeconds,
                    30);
        }

        public static int GetMaxRetryCount(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsIntValue(
                    ConnectionStringConstants.MaxRetryCount,
                    2);
        }

        public static int GetMaxRetryDelaySeconds(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsIntValue(
                    ConnectionStringConstants.MaxRetryDelaySeconds,
                    30);
        }

        public static int GetMinimumConnectionPoolSize(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsIntValue(
                    ConnectionStringConstants.MinimumConnectionPoolSize,
                    30);
        }

        public static int GetMaximumConnectionPoolSize(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsIntValue(
                    ConnectionStringConstants.MaximumConnectionPoolSize,
                    30);
        }

        public static bool GetMultipleActiveResultSets(this IConfiguration configuration)
        {
            return configuration
                .GetConnectionStringsBoolValue(
                    ConnectionStringConstants.MultipleActiveResultSets,
                    false);
        }

        internal static int GetConnectionStringsIntValue(
            this IConfiguration configuration,
            string valueTag,
            int defaultValue)
        {
            return configuration
                .GetIntValue(
                    ConnectionStringConstants.ConnectionStrings,
                    valueTag,
                    defaultValue);
        }

        internal static bool GetConnectionStringsBoolValue(
            this IConfiguration configuration,
            string valueTag,
            bool defaultValue)
        {
            return configuration
                .GetBoolValue(
                    ConnectionStringConstants.ConnectionStrings,
                    valueTag,
                    defaultValue);
        }
    }
}