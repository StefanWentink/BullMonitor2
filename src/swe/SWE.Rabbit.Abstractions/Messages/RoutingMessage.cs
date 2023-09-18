namespace SWE.Rabbit.Abstractions.Messages
{
    public class RoutingMessage<T>
    {
        public RoutingMessage(string routingKey, T value)
        {
            RoutingKey = routingKey;
            Value = value;
        }

        public string RoutingKey { get; set; }
        public T Value { get; set; }
    }
}