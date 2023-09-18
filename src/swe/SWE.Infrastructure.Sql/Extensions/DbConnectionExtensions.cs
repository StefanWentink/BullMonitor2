using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SWE.Infrastructure.Sql.Extensions
{
    internal static class DbConnectionExtensions
    {
        private static readonly ICollection<ConnectionState> _validClosedConnectionStates =
            new HashSet<ConnectionState>
            {
                ConnectionState.Closed
            };

        private static readonly ICollection<ConnectionState> _validOpenedConnectionStates =
            new HashSet<ConnectionState>
            {
                ConnectionState.Connecting,
                ConnectionState.Executing,
                ConnectionState.Fetching,
                ConnectionState.Open
            };

        internal static void CloseConnection(
            this DbConnection connection,
            ILogger? logger = default)
        {
            if (connection.ShouldCloseConnection())
            {
                logger?.LogInformation($"{nameof(connection)} closed on dispose.");
                connection.Close();
            }
        }

        internal static Task CloseConnectionAsync(
            this DbConnection connection,
            ILogger? logger = default)
        {
            if (connection.ShouldCloseConnection())
            {
                logger?.LogInformation($"{nameof(connection)} closed on dispose.");
                return connection.CloseAsync();
            }

            return Task.CompletedTask;
        }

        internal static bool ShouldCloseConnection(this DbConnection connection)
        {
            if (connection == null)
            {
                return false;
            }

            return !_validClosedConnectionStates.Contains(connection.State);
        }

        internal static bool ShouldOpenConnection(this DbConnection connection)
        {
            if (connection == null)
            {
                return false;
            }

            return !_validOpenedConnectionStates.Contains(connection.State)
                && connection.State != ConnectionState.Broken;
        }
    }
}
