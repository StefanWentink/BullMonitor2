using Microsoft.Extensions.Logging;

namespace SWE.Infrastructure.Abstractions.Interfaces.Data
{
    public interface IIssue
    {
        string ProcessName { get; }

        LogLevel LogLevel { get; }

        string Description { get; }

        /// <summary>
        /// Name of the entity that caused it
        /// </summary>
        string? EntityName { get; }

        /// <summary>
        /// Used to carry the Id of the entity that caused it
        /// </summary>
        string? Key { get; }

        /// <summary>
        /// End of range
        /// </summary>
        DateTimeOffset CreatedOn { get; }

        /// <summary>
        /// Start of range
        /// </summary>
        DateTimeOffset? From { get; }

        /// <summary>
        /// End of range
        /// </summary>
        DateTimeOffset? Until { get; }

        string? Details { get; }
    }
}