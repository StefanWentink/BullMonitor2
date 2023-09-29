using Microsoft.Extensions.Logging;
using SWE.RabbitMq.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using SWE.Rabbit.Abstractions.Messages;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.RabbitMq.Contracts
{
    public class RabbitSender<T>
        : RabbitExchange<T>
        , IMessageSender<T>
    {
        public RabbitSender(
            IRabbitConfiguration configuration,
            ILogger<RabbitSender<T>> logger)
            : base(configuration, logger)
        { }

        public Task Send(
            T value,
            CancellationToken cancellationToken)
        {
            return Send(value, string.Empty, cancellationToken);
        }

        public virtual Task Send(
            T value,
            string route,
            CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Serialize(value, SerializerOptions);
            var body = Encoding.UTF8.GetBytes(message);

            Channel.
                BasicPublish(
                    exchange: ExchangeConfiguration.ExchangeName,
                    routingKey: route,
                    basicProperties: null,
                    body: body);

            Logger.LogInformation($"{typeof(T).Name} was send: {message}");

            return Task.CompletedTask;
        }

        public async Task Send(
            RoutingMessage<T> value,
            CancellationToken cancellationToken)
        {
            await Send(value.Value, value.RoutingKey, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
