using Microsoft.Extensions.Logging;
using SWE.RabbitMq.Extensions;
using SWE.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using SWE.Extensions.Interfaces;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Issue.Abstractions.Messages;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Messages;
using SWE.Infrastructure.Abstractions.Factories;
using SWE.Issue.Abstractions.Enumerations;

namespace SWE.RabbitMq.Contracts
{
    public class RabbitReceiver<T>
        : RabbitExchange<T>
        , IMessageReceiver<T>
    {
        protected IMessageHandler<T> Handler { get; }
        protected IMessageSender<IssueMessage> IssueSender { get; }
        protected IDateTimeOffsetNow DateTimeOffsetNow { get; }

        public RabbitReceiver(
            IMessageHandler<T> handler,
            IMessageSender<IssueMessage> issueSender,
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
                    var response = JsonSerializer.Deserialize<T>(message, SerializerOptions);

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
                    IssueClassification.UnDeliverable,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task SendUnAcknowledgedIssue(string message, CancellationToken cancellationToken)
        {
            await SendIssue(
                    $"{typeof(T)} was redelivered but not acknowledged.",
                    message,
                    LogLevel.Warning,
                    IssueClassification.UnDeliverable,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task SendIssue(
            string description,
            string message,
            LogLevel logLevel,
            IssueClassification classification,
            CancellationToken cancellationToken)
        {
            var issueMessage = new IssueMessage(
                GetType().Name,
                logLevel,
                description,
                classification,
                DateTimeOffsetNow.Now,
                message);

            var routingMessage = new RoutingMessage<IssueMessage>(
                logLevel.ToString(),
                issueMessage);

            await IssueSender
                .Send(
                    routingMessage,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
