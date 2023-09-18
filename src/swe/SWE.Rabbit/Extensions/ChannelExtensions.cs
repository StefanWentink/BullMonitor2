using MoreLinq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SWE.Rabbit.Abstractions.Enums;

namespace SWE.RabbitMq.Extensions
{
    internal static class ChannelExtensions
    {
        internal static bool HandleMessageResponse(
            this IModel channel,
            BasicDeliverEventArgs basicDeliverEventArgument,
            MessageHandlingResponse messageHandlingResponse)
        {
            return messageHandlingResponse switch
            {
                MessageHandlingResponse.Successful =>
                    channel.Acknowledge(basicDeliverEventArgument),

                MessageHandlingResponse.Unsuccessful =>
                    channel.UnAcknowledge(basicDeliverEventArgument),

                MessageHandlingResponse.Rejected =>
                    channel.Reject(basicDeliverEventArgument), // Do not requeue

                _ =>
                    throw new ArgumentOutOfRangeException(
                            nameof(messageHandlingResponse),
                            messageHandlingResponse,
                            $"{nameof(ChannelExtensions)}.{nameof(HandleMessageResponse)}: { string.Join(", ", Enum.GetValues<MessageHandlingResponse>().Select(x => x.ToString())) }")
            };
        }

        internal static bool Acknowledge(
            this IModel channel,
            BasicDeliverEventArgs basicDeliverEventArgument)
        {
            channel.BasicAck(deliveryTag: basicDeliverEventArgument.DeliveryTag, multiple: false);

            return true;
        }

        internal static bool UnAcknowledge(
            this IModel channel,
            BasicDeliverEventArgs basicDeliverEventArgument)
        {
            channel.BasicNack(basicDeliverEventArgument.DeliveryTag, false, !basicDeliverEventArgument.Redelivered);

            return !basicDeliverEventArgument.Redelivered;
        }

        internal static bool Reject(
            this IModel channel,
            BasicDeliverEventArgs basicDeliverEventArgument)
        {
            channel.BasicNack(basicDeliverEventArgument.DeliveryTag, false, false);

            return true;
        }
    }
}