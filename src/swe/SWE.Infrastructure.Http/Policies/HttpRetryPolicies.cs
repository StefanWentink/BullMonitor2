using Microsoft.Extensions.Logging;
using SWE.Issue.Abstraction.Extensions;
using Polly;
using SWE.Issue.Abstraction.Enumerations;
using SWE.Issue.Abstraction.Messages;
using SWE.Infrastructure.Http.Interfaces;
using System.Net;
using SWE.Rabbit.Abstractions.Interfaces;

namespace SWE.Infrastructure.Web.Policies
{
    public static class HttpRetryPolicies
    {
        private static readonly ICollection<HttpStatusCode> _httpStatusCodesToRetry = new HashSet<HttpStatusCode>
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public static IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy(
            IMessageSender<ExceptionMessage> exceptionMessageSender,
            ILogger logger,
            IRetryPolicyConfiguration retryPolicyConfiguration,
            CancellationToken cancellationToken)
        {
            return Policy
                .Handle<TimeoutException>(exception =>
                {
                    logger.LogInformation($"{typeof(TimeoutException).Name} => {exception.Message}");
                    return true;
                })
                .OrResult<HttpResponseMessage>(httpResponseMessage =>
                {
                    var response = _httpStatusCodesToRetry.Contains(httpResponseMessage.StatusCode);

                    if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var (requestContent, responseContent) = httpResponseMessage.GetContent();

                        var message = $"{typeof(HttpResponseMessage).Name} => {httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}: {httpResponseMessage.RequestMessage.Method}: {httpResponseMessage.RequestMessage.RequestUri}"
                            + $"{nameof(requestContent)} => {requestContent}"
                            + $"{nameof(responseContent)} => {responseContent}";

                        SendIssue(
                            LogLevel.Error,
                            message,
                            ExceptionClassification.UnDeliverable,
                            exceptionMessageSender,
                            cancellationToken);

                        logger.LogError(message);

                    }
                    else if (_httpStatusCodesToRetry.Contains(httpResponseMessage.StatusCode))
                    {
                        var message = $"{typeof(HttpResponseMessage).Name} => {httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}";

                        SendIssue(
                            LogLevel.Warning,
                            message,
                            ExceptionClassification.UnDeliverable,
                            exceptionMessageSender,
                            cancellationToken);

                        logger.LogWarning(message);
#if DEBUG
                        var (requestContent, responseContent) = httpResponseMessage.GetContent();
                        logger.LogError($"{nameof(requestContent)} => {requestContent}");
                        logger.LogError($"{nameof(responseContent)} => {responseContent}");
# endif
                    }

                    return response;
                })
                .Or<HttpRequestException>(exception =>
                {
                    SendExceptionIssue(exception, exceptionMessageSender, logger, cancellationToken);

                    return false;
                })
                .WaitAndRetryAsync(
                    retryPolicyConfiguration.RetryCount,
                    retryAttempt => CalculateDelay(retryPolicyConfiguration, retryAttempt),
                    onRetry: (response, delay, retryAttempt, context) =>
                    {
                        context["retryAttempt"] = retryAttempt;
                        var message = $".WaitAndRetryAsync; Call Failed, {response.Result?.ReasonPhrase}; waiting {delay.TotalSeconds} seconds before retry attempt {retryAttempt}; Exception: {response?.Exception}";

                        SendIssue(
                            LogLevel.Error,
                            message,
                            ExceptionClassification.UnDeliverable,
                            exceptionMessageSender,
                            cancellationToken);

                        logger.LogError(message);

                    }
                 );
        }

        private static TimeSpan CalculateDelay(IRetryPolicyConfiguration policyConfig, int retryAttempt)
        {
            var calculatedPotentialDelay = TimeSpan.FromSeconds(Math.Pow(policyConfig.BreakDurationInSeconds, retryAttempt));

            if (!policyConfig.BreakDurationExponential)
            {
                var delay = 0;

                for (int i = 1; i <= retryAttempt; i++)
                {
                    delay += (i * policyConfig.BreakDurationInSeconds);
                }

                calculatedPotentialDelay = TimeSpan.FromSeconds(delay);
            }

            if (policyConfig.MaxTotalRetryDurationInSeconds != null)
            {
                // the last request after latest retry could also time out and should also not pass maximumduration, which means it should be counted as part of the potential wait time without calculated delays
                var maximumPotentialWaitTimeByClient = policyConfig.ClientRequestTimeoutInSeconds * (policyConfig.RetryCount + 1);
                // calculate possible remaining seconds to distribute between retries
                var remainderAfterSubstractFromMaxiumTotalWaitTime = policyConfig.MaxTotalRetryDurationInSeconds.Value - maximumPotentialWaitTimeByClient;
                var maximumWaitTimeBetweenRequests = remainderAfterSubstractFromMaxiumTotalWaitTime / policyConfig.RetryCount;

                if (calculatedPotentialDelay.TotalSeconds > maximumWaitTimeBetweenRequests.Value)
                {
                    return TimeSpan.FromSeconds(Convert.ToDouble(maximumWaitTimeBetweenRequests.Value));
                }
            }

            return calculatedPotentialDelay;
        }

        private static void SendIssue(
            ExceptionMessage message,
            IMessageSender<ExceptionMessage> ExceptionMessageSender,
            CancellationToken cancellationToken)
        {
            ExceptionMessageSender
                .Send(message.ToRoutingMessage(), cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        private static void SendIssue(
            LogLevel logLevel,
            string description,
            ExceptionClassification classification,
            IMessageSender<ExceptionMessage> ExceptionMessageSender,
            CancellationToken cancellationToken)
        {
            var message = new ExceptionMessage(
                nameof(GetHttpRetryPolicy),
                logLevel,
                description,
                classification,
                DateTimeOffset.Now);

            SendIssue(message, ExceptionMessageSender, cancellationToken);
        }

        private static void SendExceptionIssue(
            System.Exception exception,
            IMessageSender<ExceptionMessage> ExceptionMessageSender,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            var message = new ExceptionMessage(
                nameof(GetHttpRetryPolicy),
                LogLevel.Error,
                exception.Message,
                ExceptionClassification.Exception,
                DateTimeOffset.Now);

            SendIssue(message, ExceptionMessageSender, cancellationToken);
        }
    }
}