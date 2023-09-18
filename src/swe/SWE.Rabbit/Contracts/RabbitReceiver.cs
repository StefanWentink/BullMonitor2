using Microsoft.Extensions.Logging;
using SWE.RabbitMq.Extensions;
using SWE.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using SWE.Extensions.Interfaces;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Rabbit.Abstractions.Enums;
using SWE.Rabbit.Abstractions.Messages;

namespace SWE.RabbitMq.Contracts
{
    public class RabbitReceiver<T>
        : RabbitExchange<T>
        , IMessageReceiver<T>
    {
        protected IMessageHandler<T> Handler { get; }
        protected IMessageSender<IssueModel> IssueSender { get; }
        protected IDateTimeOffsetNow DateTimeOffsetNow { get; }

        public RabbitReceiver(
            IMessageHandler<T> handler,
            IMessageSender<IssueModel> issueSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            IRabbitConfiguration configuration,
            ILogger<RabbitReceiver<T>> logger)
            : base(configuration, logger)
        {
            Handler = handler;
            IssueSender = issueSender;
            DateTimeOffsetNow = dateTimeOffsetNow;
        }

        public Task StartReceiving(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += async (model, basicDeliverEventArgument) =>
            {
                var body = basicDeliverEventArgument.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    Console.WriteLine($"[{typeof(T).Name}] Received {message}");
                    var response = JsonSerializer.Deserialize<T>(message);

                    try
                    {
                        var messageHandlingResponse = await Handler
                            .Handle(response, cancellationToken)
                            .ConfigureAwait(false);

                        if (!ExchangeConfiguration.ReceiveAndDelete
                            && !Channel.HandleMessageResponse(basicDeliverEventArgument, messageHandlingResponse))
                        {
                            await SendUnAcknowledgedIssue(message, cancellationToken)
                                .ConfigureAwait(false);
                        }

                    }
                    catch (Exception exception)
                    {
                        Logger.LogError(exception, exception.Message);
                        if (!ExchangeConfiguration.ReceiveAndDelete)
                        {
                            // Handling went wrong => should try again (later)
                            if (!ExchangeConfiguration.ReceiveAndDelete
                                && !Channel.HandleMessageResponse(basicDeliverEventArgument, MessageHandlingResponse.Unsuccessful))
                            {
                                await SendUnAcknowledgedIssue(message, cancellationToken)
                                    .ConfigureAwait(false);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.LogError(exception, exception.Message);

                    if (!ExchangeConfiguration.ReceiveAndDelete)
                    {
                        // Deserializing went wrong
                        Channel.Reject(basicDeliverEventArgument);

                        await SendRejectedIssue(message, cancellationToken)
                                    .ConfigureAwait(false);
                    }
                }
            };

            foreach (var queue in ExchangeConfiguration.Queues)
            {
                Channel
                    .BasicConsume(
                        queue: queue.QueueName,
                        autoAck: ExchangeConfiguration.ReceiveAndDelete,
                        consumer: consumer);
            }

            return Task.CompletedTask;
        }

        private async Task SendRejectedIssue(string message, CancellationToken cancellationToken)
        {
            await SendIssue(
                    $"{typeof(T)} was undeliverable.",
                    message,
                    LogLevel.Critical,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task SendUnAcknowledgedIssue(string message, CancellationToken cancellationToken)
        {
            await SendIssue(
                    $"{typeof(T)} was redelivered but not acknowledged.",
                    message,
                    LogLevel.Warning,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task SendIssue(
            string description,
            string message,
            LogLevel logLevel,
            CancellationToken cancellationToken)
        {
            var issueModel = new IssueModel(
                GetType().Name,
                logLevel,
                description,
                DateTimeOffsetNow.Now,
                message);

            var routingMessage = new RoutingMessage<IssueModel>(
                logLevel.ToString(),
                issueModel);

            await IssueSender
                .Send(
                    routingMessage,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
