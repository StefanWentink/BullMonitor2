using SWE.Infrastructure.Sql.Constants;

namespace SWE.Infrastructure.Sql.Extensions
{
    internal static class ConnectionStringExtensions
    {
        internal static string AppendMaxConnectionPoolSize(
            this string connectionString,
            int maxConnectionPoolSize)
        {
            if (maxConnectionPoolSize <= 0
                || connectionString.Contains(ConnectionStringConstants.MaxPoolSize, StringComparison.OrdinalIgnoreCase))
            {
                return connectionString;
            }

            return connectionString +
                (connectionString.Trim().EndsWith(';') ? "" : ";")
                + ConnectionStringConstants.MaxConnectionPoolSize(maxConnectionPoolSize);
        }

        internal static string AppendMinConnectionPoolSize(
            this string connectionString,
            int minConnectionPoolSize)
        {
            if (minConnectionPoolSize <= 0
                || connectionString.Contains(ConnectionStringConstants.MinPoolSize, StringComparison.OrdinalIgnoreCase))

            {
                return connectionString;
            }

            return connectionString +
                (connectionString.Trim().EndsWith(';') ? "" : ";")
                + ConnectionStringConstants.MinConnectionPoolSize(minConnectionPoolSize);
        }

        internal static string AppendMultipleActiveResultSets(
            this string connectionString,
            bool multipleActiveResultSets)
        {
            if (!multipleActiveResultSets
                || connectionString.Contains(ConnectionStringConstants.MultipleActiveResultSets, StringComparison.OrdinalIgnoreCase))

            {
                return connectionString;
            }

            return connectionString +
                (connectionString.Trim().EndsWith(';') ? "" : ";")
                + ConnectionStringConstants.MultipleActiveResultSet(multipleActiveResultSets);
        }
    }
}
