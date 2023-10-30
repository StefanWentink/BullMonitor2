using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using BullMonitor.Abstractions.Utilities;

namespace BullMonitor.DataMine.Providers
{
    public class ZacksRawProvider
        : ISingleProvider<ZacksRequest, string?>
    {        // Set the URL
        private static string _url = "https://www.zacks.com/stock/research/";

        private static IAsyncPolicy<HttpResponseMessage> _retryPolicy = HttpPolicyExtensions
             .HandleTransientHttpError()
             .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        protected IHttpClientFactory ClientFactory { get; }
        protected ILogger<ZacksRawProvider> Logger { get; }

        public ZacksRawProvider(
            IHttpClientFactory clientFactory,
            ILogger<ZacksRawProvider> logger)
        {
            ClientFactory = clientFactory;
            Logger = logger;
        }

        public async Task<string?> GetSingleOrDefault(
            ZacksRequest value,
            CancellationToken cancellationToken)
        {
            try
            {
                // Create an instance of the HttpClient class using the retry policy
                using var client = ClientFactory
                    .CreateClient("apiClient");

                var ticker = TickerUtilities.CleanTicker(value.Ticker);

                client.BaseAddress = new Uri(string.Format(_url, ticker));

                // Send the GET request and store the response
                var responseMessage = await _retryPolicy
                    .ExecuteAsync(() => client.GetAsync($"{ticker}/price-target-stock-forecast", cancellationToken))
                    .ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    // Read the response content
                    return await responseMessage
                        .Content
                        .ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                Logger.LogError($"{_url}/?name={ticker} resulted in {responseMessage.StatusCode}:{responseMessage.ReasonPhrase}");
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
            }

            return null;
        }
    }
}