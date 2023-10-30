using Microsoft.Extensions.Logging;
using SWE.Issue.Abstractions.Extensions;
using Polly;
using Polly.CircuitBreaker;
using SWE.Issue.Abstractions.Enumerations;
using SWE.Issue.Abstractions.Messages;
using SWE.Infrastructure.Http.Interfaces;
using System.Net;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Infrastructure.Web.Policies
{
    public class HttpCircuitBreakerPolicies
    {
        private static readonly ICollection<HttpStatusCode> _httpStatusCodesToRetry = new HashSet<HttpStatusCode>
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public static IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(
            IMessageSender<IssueMessage> issueMessageSender,
            ILogger logger,
            ICircuitBreakerPolicyConfiguration circuitBreakerPolicyConfiguration,
            CancellationToken cancellationToken)
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (response, retryCount) =>
            {
                SendIssue(
                    LogLevel.Warning,
                    $".CircuitBreaker; Breaking the circuit for {retryCount.TotalMilliseconds} ms; {response.Result?.ReasonPhrase}; Exception: {response.Exception?.Message}",
                    IssueClassification.UnDeliverable,
                    issueMessageSender,
                    cancellationToken);
            };

            Action onReset = () =>
            {
                SendIssue(
                    LogLevel.Warning,
                    $".CircuitBreaker; Call ok! Closed the circuit again!",
                    IssueClassification.UnDeliverable,
                    issueMessageSender,
                    cancellationToken);
            };

            Action onHalfOpen = () =>
            {
                SendIssue(
                    LogLevel.Warning,
                    $".CircuitBreaker; Half-open: Next call is a trial!",
                    IssueClassification.UnDeliverable,
                    issueMessageSender,
                    cancellationToken);
            };

            return Policy
                .Handle<Exception>(exception =>
                {
                    SendExceptionIssue(exception, issueMessageSender, logger, cancellationToken);

                    return exception is not BrokenCircuitException;
                })
                .OrResult<HttpResponseMessage>(httpResponseMessage =>
                {
                    var isRetryStatusCode = _httpStatusCodesToRetry.Contains(httpResponseMessage.StatusCode);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var (requestContent, responseContent) = httpResponseMessage.GetContent();

                        var message = $"{typeof(HttpResponseMessage).Name} => {httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}: {httpResponseMessage.RequestMessage.Method}: {httpResponseMessage.RequestMessage.RequestUri}" +
                        $"{nameof(requestContent)} => {requestContent}" +
                        $"{nameof(responseContent)} => {responseContent}";

                        SendIssue(
                            LogLevel.Warning,
                            message,
                            IssueClassification.UnDeliverable,
                            issueMessageSender,
                            cancellationToken);

                        logger.LogError($"{typeof(HttpResponseMessage).Name} => {httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}: {httpResponseMessage.RequestMessage.Method}: {httpResponseMessage.RequestMessage.RequestUri}");
                        logger.LogError($"{nameof(requestContent)} => {requestContent}");
                        logger.LogError($"{nameof(responseContent)} => {responseContent}");
                    }
                    else if (isRetryStatusCode)
                    {
                        SendIssue(
                            LogLevel.Warning,
                            $"{typeof(HttpResponseMessage).Name} => {httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}.{Environment.NewLine}{httpResponseMessage.Content}",
                            IssueClassification.UnDeliverable,
                            issueMessageSender,
                            cancellationToken);

#if DEBUG
                        var (requestContent, responseContent) = httpResponseMessage.GetContent();

                        SendIssue(
                            LogLevel.Warning,
                            $"{nameof(requestContent)} => {requestContent} {Environment.NewLine} {nameof(responseContent)} => {responseContent}",
                            IssueClassification.UnDeliverable,
                            issueMessageSender,
                            cancellationToken);

                        logger.LogError($"{nameof(requestContent)} => {requestContent}");
                        logger.LogError($"{nameof(responseContent)} => {responseContent}");
# endif
                    }

                    return isRetryStatusCode;
                })
                .CircuitBreakerAsync(
                    circuitBreakerPolicyConfiguration.RetryCount,
                    TimeSpan.FromSeconds(circuitBreakerPolicyConfiguration.BreakDurationInSeconds),
                    onBreak,
                    onReset,
                    onHalfOpen);
        }

        protected static void SendIssue(
            IssueMessage message,
            IMessageSender<IssueMessage> IssueMessageSender,
            CancellationToken cancellationToken)
        {
            IssueMessageSender
                .Send(message.ToRoutingMessage(), cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        protected static void SendIssue(
            LogLevel logLevel,
            string description,
            IssueClassification classification,
            IMessageSender<IssueMessage> IssueMessageSender,
            CancellationToken cancellationToken)
        {
            var message = new IssueMessage(
                nameof(GetHttpCircuitBreakerPolicy),
                logLevel,
                description,
                classification,
                DateTimeOffset.Now);

            SendIssue(message, IssueMessageSender, cancellationToken);
        }

        protected static void SendExceptionIssue(
            System.Exception exception,
            IMessageSender<IssueMessage> IssueMessageSender,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            var message = new IssueMessage(
                nameof(GetHttpCircuitBreakerPolicy),
                LogLevel.Error,
                exception.Message,
                IssueClassification.Exception,
                DateTimeOffset.Now);

            SendIssue(message, IssueMessageSender, cancellationToken);
        }
    }
}