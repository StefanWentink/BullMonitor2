using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Data;
using System.Text;

namespace SWE.Infrastructure.Abstractions.Models
{
    public class IssueModel
        : IIssue
    {
        public string ProcessName { get; protected set; }
        public LogLevel LogLevel { get; protected set; }
        public string Description { get; protected set; }
        public string? EntityName { get; protected set; }
        public string? Key { get; protected set; }
        public DateTimeOffset CreatedOn { get; protected set; }
        public DateTimeOffset? From { get; protected set; }
        public DateTimeOffset? Until { get; protected set; }
        public string? Details { get; protected set; }

        [Obsolete("Only for serialization.")]
        public IssueModel()
            : this(
                  string.Empty,
                  LogLevel.None,
                  string.Empty,
                  default,
                  null)
        { }

        public IssueModel(
            string processName,
            LogLevel logLevel,
            string description,
            DateTimeOffset createdOn,
            string? details = null)
            : this(
                processName,
                logLevel,
                description,
                null,
                null,
                createdOn,
                null,
                null,
                details)
        { }

        public IssueModel(
            string processName,
            LogLevel logLevel,
            string description,
            DateTimeOffset createdOn,
            DateTimeOffset? from,
            DateTimeOffset? until,
            string? details = null)
            : this(
                processName,
                logLevel,
                description,
                null,
                null,
                createdOn,
                from,
                until,
                details)
        { }

        public IssueModel(
            string processName,
            LogLevel logLevel,
            string description,
            string? entityName,
            string? key,
            DateTimeOffset createdOn,
            string? details = null)
            : this(
                processName,
                logLevel,
                description,
                entityName,
                key,
                createdOn,
                null,
                null,
                details)
        { }


        public IssueModel(
            string processName,
            LogLevel logLevel,
            string description,
            string? entityName,
            string? key,
            DateTimeOffset createdOn,
            DateTimeOffset? from,
            DateTimeOffset? until,
            string? details = null)
        {
            ProcessName = processName;
            LogLevel = logLevel;
            Description = description;
            EntityName = entityName;
            Key = key;
            CreatedOn = createdOn;
            From = from;
            Until = until;
            Details = details ?? GetDetails();
        }

        public string GetDetails()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{ProcessName}[{LogLevel}-{CreatedOn}]");
            stringBuilder.AppendLine($"{Description}");

            if (!string.IsNullOrWhiteSpace(EntityName) || !string.IsNullOrWhiteSpace(Key))
            {
                stringBuilder.AppendLine($"{EntityName}[{Key}]");
            }

            if (!string.IsNullOrWhiteSpace(EntityName) || !string.IsNullOrWhiteSpace(Key))
            {
                stringBuilder.AppendLine($"{EntityName}[{Key}]");
            }

            if (From != null || Until != null)
            {
                stringBuilder.AppendLine($"{From}=>{Until}]");
            }

            return stringBuilder.ToString();
        }
    }
}
