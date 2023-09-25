using BullMonitor.Ticker.Process.Models;

namespace BullMonitor.Ticker.Process.Utilities
{
    internal static class CsvUtilities
    {
        private static string _fileName = "{0}.csv";

        internal static async Task<IEnumerable<FileTickerModel>> Read(
            string exchange,
            CancellationToken cancellationToken)
        {
            var responseCollection = new HashSet<FileTickerModel>();

            var path = AppDomain.CurrentDomain.BaseDirectory;

            if (string.IsNullOrWhiteSpace(path))
            {
                return Enumerable.Empty<FileTickerModel>();
            }

            var filePath = string
                .Join("\\", path, "Resources", string.Format(_fileName, exchange))
                .Replace("\\\\", "\\");

            var text = await File
                .ReadAllTextAsync(filePath, cancellationToken)
                .ConfigureAwait(false);

            return TextToTicker(text, exchange);
        }
        
        internal static IEnumerable<FileTickerModel> TextToTicker(
            string text,
            string exchange)
        {
            var responseCollection = new HashSet<FileTickerModel>();

            foreach (var line in text
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                var split = line.Split(';');
                var response = new FileTickerModel { Exchange = exchange };

                if (split.Length >= 1)
                {
                    response.Ticker = split[0].Trim();
                }
                if (split.Length >= 2)
                {
                    response.Name = split[1].Trim();
                }
                if (split.Length >= 3)
                {
                    response.Country = split[2].Trim();
                }
                if (split.Length >= 4)
                {
                    response.Sector = split[3].Trim();
                }
                if (split.Length >= 5)
                {
                    response.Industry = split[4].Trim();
                }

                responseCollection.Add(response);
            }

            return responseCollection;
        }
    }
}