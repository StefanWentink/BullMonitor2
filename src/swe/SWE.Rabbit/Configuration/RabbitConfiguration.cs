using SWE.Extensions.Utilities;
using SWE.Rabbit.Abstractions.Enums;
using SWE.RabbitMq.Interfaces;

namespace SWE.Rabbit.Configuration
{
    public class RabbitConfiguration
        : IRabbitConfiguration
    {
        private ICollection<IRabbitExchangeConfiguration>? _exchanges = null;

        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName(nameof(Exchanges))]
        //[Newtonsoft.Json.JsonProperty(nameof(Exchanges))]
        public RabbitExchangeConfiguration[] Exchange { get; set; } = Array.Empty<RabbitExchangeConfiguration>();

        [System.Text.Json.Serialization.JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public ICollection<IRabbitExchangeConfiguration> Exchanges =>
            _exchanges ??= (Exchange?.Cast<IRabbitExchangeConfiguration>() ?? Enumerable.Empty<IRabbitExchangeConfiguration>()).ToList();

        public IRabbitExchangeConfiguration Get(string exchangeName)
        {
            return Exchanges
                .SingleOrDefault(x => x.Name.Equals(exchangeName, StringComparison.OrdinalIgnoreCase))
                ?? throw new NotSupportedException($"No {nameof(Exchanges)} with {nameof(exchangeName)} '{exchangeName}' found.");
        }

        public void AddExchange(RabbitExchangeConfiguration exchange)
        {
            Exchanges.Add(exchange);
        }
    }

    public class RabbitExchangeConfiguration
        : IRabbitExchangeConfiguration
    {
        private ICollection<IRabbitQueueConfiguration>? _queues = null;

        [Obsolete("Only for serialisation.")]
        public RabbitExchangeConfiguration()
            : this(
                  string.Empty,
                  Enumerable.Empty<RabbitQueueConfiguration>())
        { }

        public RabbitExchangeConfiguration(
            string name,
            IEnumerable<RabbitQueueConfiguration> queues)
            : this(
                  name,
                  queues,
                  MessageExchangeType.Direct.ToString(),
                  false)
        { }

        public RabbitExchangeConfiguration(
            string name,
            IEnumerable<RabbitQueueConfiguration> queues,
            string exchangeType,
            bool receiveAndDelete = false)
        {
            Name = name;
            Queue = queues.ToArray();
            ExchangeType = exchangeType;
            ReceiveAndDelete = receiveAndDelete;
        }

        public string Name { get; set; }
        public string ExchangeName => $"x_{Name}";
        public string ExchangeType { get; set; } = MessageExchangeType.Direct.ToString();
        public MessageExchangeType Type => EnumUtilities.GetEnumValue<MessageExchangeType>(ExchangeType);
        public bool ReceiveAndDelete { get; set; } = false;

        [System.Text.Json.Serialization.JsonPropertyName(nameof(Queues))]
        //[Newtonsoft.Json.JsonProperty(nameof(Queues))]
        public RabbitQueueConfiguration[] Queue { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        public ICollection<IRabbitQueueConfiguration> Queues => _queues ??= Queue.Cast<IRabbitQueueConfiguration>().ToList();
        
        public IRabbitQueueConfiguration Get(string queueName)
        {
            return Queues.Single(x => x.Name.Equals(queueName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class RabbitQueueConfiguration
        : IRabbitQueueConfiguration
    {
        [Obsolete("only for serialisation.")]
        public RabbitQueueConfiguration()
            : this(
                 string.Empty)
        { }

        public RabbitQueueConfiguration(
            string name)
            : this(
                 name,
                 string.Empty,
                 false,
                 true)
        { }

        public RabbitQueueConfiguration(
            string name,
            string routingKey,
            bool autoDelete = false,
            bool durable = true)
        {
            Name = name;
            AutoDelete = autoDelete;
            Durable = durable;
            RoutingKey = routingKey;
        }

        public string Name { get; set; }
        public string QueueName => $"q_{Name}";

        public bool AutoDelete { get; set; } = false;
        public bool Durable { get; set; } = true;
        public string RoutingKey { get; set; } = string.Empty;
    }
}