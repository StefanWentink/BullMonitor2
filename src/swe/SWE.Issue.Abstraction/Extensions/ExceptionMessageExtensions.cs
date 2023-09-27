using SWE.Issue.Abstraction.Messages;
using SWE.Rabbit.Abstractions.Messages;

namespace SWE.Issue.Abstraction.Extensions
{
    public static class ExceptionMessageExtensions
    {
        public static RoutingMessage<ExceptionMessage> ToRoutingMessage(
            this ExceptionMessage message)
        {
            return new RoutingMessage<ExceptionMessage>(
                message.Classification.ToString(),
                message);
        }
    }
}