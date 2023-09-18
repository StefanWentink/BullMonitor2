using RabbitMQ.Client;
using SWE.Rabbit.Abstractions.Enums;

namespace SWE.RabbitMq.Interfaces
{
    public interface IRabbitExchangeConfiguration
    {
        string Name { get; set; }

        string ExchangeName { get; }

        /// <summary>
        /// can either be <see cref="ExchangeType.Fanout"/>, <see cref="ExchangeType.Direct"/>, <see cref="ExchangeType.Topic"/> or <see cref="ExchangeType.Headers"/>
        /// </summary>
        MessageExchangeType Type { get; }

        /// <summary>
        /// Does consuming the message send <see cref="MessageHandlingResponse.Successful"/>.
        /// </summary>
        bool ReceiveAndDelete { get; }

        IRabbitQueueConfiguration Get(string exchangeName);

        ICollection<IRabbitQueueConfiguration> Queues { get; }
    }
}