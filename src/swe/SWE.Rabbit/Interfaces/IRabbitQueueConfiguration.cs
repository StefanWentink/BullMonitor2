namespace SWE.RabbitMq.Interfaces
{
    public interface IRabbitQueueConfiguration
    {
        string Name { get; set; }
        string QueueName { get; }
        
        /// <summary>
        /// Queue will be not automatically be created when the exchange is set up. (NS implemented behavior)
        /// Queue will be deleted when the last consumer is gone. (Rabbit behavior)
        /// </summary>
        bool AutoDelete { get; }
        bool Durable { get; }
        string RoutingKey { get; }
    }
}