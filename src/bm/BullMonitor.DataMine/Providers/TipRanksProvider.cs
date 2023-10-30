using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using System;
using System.Text.Json;

namespace BullMonitor.DataMine.Providers
{
    public class TipRanksProvider
        : ISingleProvider<TipRanksRequest, TipRanksResponse>
    {
        protected ISingleProvider<TipRanksRequest, string?> RawProvider { get; }

        public TipRanksProvider(
            ISingleProvider<TipRanksRequest, string?> rawProvider)
        {
            RawProvider = rawProvider;
        }

        public async Task<TipRanksResponse> GetSingleOrDefault(
            TipRanksRequest value,
            CancellationToken cancellationToken)
        {
            // Read the response content
            string? responseString = await RawProvider
                .GetSingleOrDefault(value, cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(responseString))
            {
                return new TipRanksResponse
                {
                    Ticker = value.Ticker,
                    Found = false
                };
            }

            // Deserialize the JSON string into an object
            var response = JsonSerializer
                .Deserialize<TipRanksResponse>(responseString);

            return response ?? new TipRanksResponse
            {
                Ticker = value.Ticker,
                Found = false
            };
        }
    }
}