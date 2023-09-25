using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System;
using BullMonitor.Abstractions.Utilities;

namespace BullMonitor.DataMine.Providers
{
    public class TipRanksRawProvider
        : ISingleProvider<TipRanksRequest, string?>
    {
        // Set the URL
        private static string _url = "https://www.tipranks.com/api/stocks/getData/";

        private static IAsyncPolicy<HttpResponseMessage> _retryPolicy = HttpPolicyExtensions
             .HandleTransientHttpError()
             .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        protected IHttpClientFactory ClientFactory { get; }
        protected ILogger<TipRanksRawProvider> Logger { get; }

        public TipRanksRawProvider(
            IHttpClientFactory clientFactory,
            ILogger<TipRanksRawProvider> logger)
        {
            ClientFactory = clientFactory;
            Logger = logger;
        }

        public async Task<string?> GetSingleOrDefault(
            TipRanksRequest value,
            CancellationToken cancellationToken)
        {
            try
            {
                // Create an instance of the HttpClient class using the retry policy
                using var client = ClientFactory
                    .CreateClient("apiClient");

                client.BaseAddress = new Uri(_url);

                // Send the GET request and store the response
                var responseMessage = await _retryPolicy
                    .ExecuteAsync(() => client.GetAsync($"?name={TickerUtilities.CleanTicker(value.Ticker)}", cancellationToken))
                    .ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    // Read the response content
                    return await responseMessage
                        .Content
                        .ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                Logger.LogError($"{_url}/?name={value.Ticker} resulted in {responseMessage.StatusCode}:{responseMessage.ReasonPhrase}");
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
            }

            return null;
        }
    }
}