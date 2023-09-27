namespace SWE.Infrastructure.Web.Policies
{
    internal static class HttpResponseMessageExtensions
    {
        internal static async Task<(string requestContent, string responseContent)> GetContentAsync(
            this HttpResponseMessage httpResponseMessage,
            CancellationToken cancellationToken)
        {
            var requestContent = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty;

            if (httpResponseMessage.RequestMessage?.Method != HttpMethod.Get
                && httpResponseMessage.RequestMessage?.Content != null)
            {
                var content = await httpResponseMessage
                    .RequestMessage
                    .Content
                    .ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                requestContent += " => " + content;
            }

            var responseContent = await httpResponseMessage
                .Content
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            return (requestContent, responseContent);
        }
        internal static (string requestContent, string responseContent) GetContent(
            this HttpResponseMessage httpResponseMessage)
        {
            return httpResponseMessage
                .GetContentAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }
    }
}