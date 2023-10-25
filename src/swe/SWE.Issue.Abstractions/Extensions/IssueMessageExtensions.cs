using SWE.Issue.Abstractions.Messages;
using SWE.Rabbit.Abstractions.Messages;

namespace SWE.Issue.Abstractions.Extensions
{
    public static class IssueMessageExtensions
    {
        public static RoutingMessage<IssueMessage> ToRoutingMessage(
            this IssueMessage message)
        {
            return new RoutingMessage<IssueMessage>(
                message.Classification.ToString(),
                message);
        }
    }
}