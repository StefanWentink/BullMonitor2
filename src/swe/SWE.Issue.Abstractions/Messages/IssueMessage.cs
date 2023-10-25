using Microsoft.Extensions.Logging;
using SWE.Issue.Abstractions.Enumerations;
using SWE.Issue.Abstractions.Interfaces;
using System.Text;

namespace SWE.Issue.Abstractions.Messages
{
    public class IssueMessage
        : IIssue
    {
        public string ProcessName { get; protected set; }
        public LogLevel LogLevel { get; protected set; }
        public string Description { get; protected set; }
        public IssueClassification Classification { get; protected set; }
        public string? EntityName { get; protected set; }
        public string? Key { get; protected set; }
        public DateTimeOffset CreatedOn { get; protected set; }
        public DateTimeOffset? From { get; protected set; }
        public DateTimeOffset? Until { get; protected set; }
        public string? Details { get; protected set; }

        [Obsolete("Only for serialization.")]
        public IssueMessage()
            : this(
                  string.Empty,
                  LogLevel.None,
                  string.Empty,
                  IssueClassification.Unknown,
                  default,
                  null)
        { }

        public IssueMessage(
            string processName,
            LogLevel logLevel,
            string description,
            IssueClassification classification,
            DateTimeOffset createdOn,
            string? details = null)
            : this(
                processName,
                logLevel,
                description,
                null,
                null,
                classification,
                createdOn,
                null,
                null,
                details)
        { }

        public IssueMessage(
            string processName,
            LogLevel logLevel,
            string description,
            IssueClassification classification,
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
                classification,
                createdOn,
                from,
                until,
                details)
        { }

        public IssueMessage(
            string processName,
            LogLevel logLevel,
            string description,
            string? entityName,
            string? key,
            IssueClassification classification,
            DateTimeOffset createdOn,
            string? details = null)
            : this(
                processName,
                logLevel,
                description,
                entityName,
                key,
                classification,
                createdOn,
                null,
                null,
                details)
        { }


        public IssueMessage(
            string processName,
            LogLevel logLevel,
            string description,
            string? entityName,
            string? key,
            IssueClassification classification,
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
            Classification = classification;
            CreatedOn = createdOn;
            From = from;
            Until = until;
            Details = details ?? GetDetails();
        }

        public string GetDetails()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{ProcessName}[{Classification}-{LogLevel}-{CreatedOn}]");
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
