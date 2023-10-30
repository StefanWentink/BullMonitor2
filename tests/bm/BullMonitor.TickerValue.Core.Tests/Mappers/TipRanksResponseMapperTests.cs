using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.TickerValue.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Tests.Base;
using System.Text.Json;
using Xunit;

namespace BullMonitor.TickerValue.Core.Tests.Mappers
{
    public class TipRanksResponseMapperTests
        : BaseServiceProviderTests<
            IMapper<
                (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
                IEnumerable<TipRanksEntity>>>
    {
        private static string _fileName = "tipranks_{0}.json";

        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorTickerValueMapperServices(configuration);
        }

        [Theory]
        [InlineData(false, 260)]
        [InlineData(true, 1)]
        public async Task TipRanksResponseMapper_Should_Map(
            bool targetDateOnly,
            int expectedPriceCount)
        {
            //var referenceDate = new DateTimeOffset(2023, 10, 26, 0, 0, 0, TimeSpan.FromHours(2));
            var referenceDate = new DateTimeOffset(2022, 12, 21, 0, 0, 0, TimeSpan.FromHours(1));
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var content = await ReadContent("MSFT", cancellationToken)
                .ConfigureAwait(false);

            var tipranksResponse = JsonSerializer
                .Deserialize<TipRanksResponse>(content);

            var mapper = Create();

            var actual = await mapper
                .Map((tipranksResponse, referenceDate, targetDateOnly), cancellationToken)
                .ConfigureAwait(false);

            actual.Should().HaveCount(expectedPriceCount);
            actual.All(x => x.Value.PriceTarget != 0m);
        }

        private async Task<string?> ReadContent(string ticker, CancellationToken cancellationToken)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("No file path found.");
                    return null;
                }

                var filePath = string
                    .Join("\\", path, "Resources", string.Format(_fileName, ticker))
                    .Replace("\\\\", "\\");

                return await File
                    .ReadAllTextAsync(filePath, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }
    }
}
