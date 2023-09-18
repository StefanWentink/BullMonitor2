namespace SWE.Infrastructure.Sql.Constants
{
    internal static class ConnectionStringConstants
    {
        internal const string ConnectionStrings = nameof(ConnectionStrings);

        internal const string MinimumConnectionPoolSize = nameof(MinimumConnectionPoolSize);
        internal const string MaximumConnectionPoolSize = nameof(MaximumConnectionPoolSize);

        internal const string MultipleActiveResultSets = nameof(MultipleActiveResultSets);
        internal const string CommandTimeoutSeconds = nameof(CommandTimeoutSeconds);
        internal const string MaxRetryCount = nameof(MaxRetryCount);
        internal const string MaxRetryDelaySeconds = nameof(MaxRetryDelaySeconds);

        internal const string MaxPoolSize = "Max Pool Size";
        internal const string MinPoolSize = "Min Pool Size";

        internal static string MultipleActiveResultSet(bool multipleActiveResultSets)
        {
            var multipleActiveResultSetsString = multipleActiveResultSets ? "True" : "False";
            return $"{MultipleActiveResultSets}={multipleActiveResultSetsString};";
        }

        internal static string MaxConnectionPoolSize(int connectionPoolSize) => $"{MaxPoolSize}={connectionPoolSize};";
        internal static string MinConnectionPoolSize(int connectionPoolSize) => $"{MinPoolSize}={connectionPoolSize};";
    }
}