using SWE.Rabbit.Configuration;

namespace SWE.RabbitMq.Interfaces
{
    public interface IRabbitConfiguration
    {
        string HostName { get; }
        string UserName { get; }
        string Password { get; }

        IRabbitExchangeConfiguration Get(string exchangeName);
        ICollection<IRabbitExchangeConfiguration> Exchanges { get; }
        void AddExchange(RabbitExchangeConfiguration exchange);
    }
}