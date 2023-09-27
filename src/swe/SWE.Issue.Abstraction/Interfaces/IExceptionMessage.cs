using Microsoft.Extensions.Logging;
using SWE.Issue.Abstraction.Enumerations;

namespace SWE.Issue.Abstraction.Interfaces
{
    public interface IExceptionMessage
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
        /// General classification of what went wrong
        /// </summary>
        ExceptionClassification Classification { get; }

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
