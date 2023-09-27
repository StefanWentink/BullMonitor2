using Microsoft.Extensions.Logging;
using System.Net;

namespace SWE.Infrastructure.Web.Extensions
{
    internal static class HttpStatusCodeExtensions
    {
        internal static bool ShouldRetry(
            this HttpResponseMessage message,
            ILogger logger)
        {
            logger.LogInformation($"{ nameof(HttpStatusCodeExtensions) }.{ nameof(ShouldRetry) } => { message.StatusCode }");
            
            return
                (message.StatusCode < HttpStatusCode.OK || message.StatusCode >= HttpStatusCode.Ambiguous)
                &&
                (
                    message.StatusCode != HttpStatusCode.Unauthorized
                    || message.StatusCode != HttpStatusCode.Forbidden
                    || message.StatusCode != HttpStatusCode.NotFound
                );
        }
    }
}