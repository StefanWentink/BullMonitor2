using BullMonitor.Abstractions.Requests;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.DataMine.Tests.Stubs
{
    internal class StubTipRanksRawProvider
        : ISingleProvider<TipRanksRequest, string?>
    {
        private static string _fileName = "tipranks_{0}.json";

        protected ILogger<StubTipRanksRawProvider> Logger { get; }

        public async Task<string?> GetSingleOrDefault(
            TipRanksRequest value,
            CancellationToken cancellationToken)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;

                if (string.IsNullOrWhiteSpace(path))
                {
                    Logger.LogWarning("No file path found.");
                    return null;
                }

                var filePath = string
                    .Join("\\", path, "Resources", string.Format(_fileName, value.Ticker))
                    .Replace("\\\\", "\\");

                return await File
                    .ReadAllTextAsync(filePath, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return null;
            }
        }
    }
}