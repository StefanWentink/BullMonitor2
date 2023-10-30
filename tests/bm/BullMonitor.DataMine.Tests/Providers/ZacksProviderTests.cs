using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.DataMine.Extensions;
using BullMonitor.DataMine.Tests.Stubs;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Tests.Base;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using Xunit;

namespace BullMonitor.DataMine.Tests.Providers
{
    public class ZacksProviderTests
        : BaseServiceProviderTests<ISingleProvider<ZacksRequest, ZacksResponse>>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorDataMineServices(configuration)
                .AddSingleton<ISingleProvider<ZacksRequest, string?>, StubZacksRawProvider>();
        }

        [Theory]
        [InlineData("MSFT")]
        [InlineData("LIZI")]
        public async Task ZacksProvider_Should_ResolveModel(
            string ticker)
        {
            var from = new DateTimeOffset(2022, 10, 30, 0, 0, 0, TimeSpan.FromHours(2));
            var until = new DateTimeOffset(2022, 10, 31, 0, 0, 0, TimeSpan.FromHours(1));
            var request = new ZacksRequest(
                from,
                until,
                ticker);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            var response = await provider
                .GetSingleOrDefault(request, cancellationToken)
                .ConfigureAwait(false);

            response.Should().NotBeNull();

            if (response != null)
            {
                response.Ticker.Should().Be(request.Ticker);

                if (response.Ticker.Equals("MSFT"))
                {
                    response.Rank.Should().Be(3);
                    response.Value.Should().Be(3);
                    response.Growth.Should().Be(1);
                    response.Momentum.Should().Be(4);
                    response.VGM.Should().Be(2);

                    response.AveragePriceTarget.Should().Be(292.36m);
                    response.HighestPriceTarget.Should().Be(370.00m);
                    response.LowestPriceTarget.Should().Be(234.00m);
                    response.PriceTargetPercentage.Should().Be(22.74m);

                    response.StrongBuy.Should().Be(24);
                    response.Buy.Should().Be(2);
                    response.Hold.Should().Be(3);
                    response.Sell.Should().Be(0);
                    response.StrongSell.Should().Be(0);
                    response.AverageBrokerRecommendation.Should().Be(1.28m);
                }
                else if (response != null && response.Ticker.Equals("LIZI"))
                {
                    response.Rank.Should().Be(0);
                    response.Value.Should().Be(5);
                    response.Growth.Should().Be(5);
                    response.Momentum.Should().Be(5);
                    response.VGM.Should().Be(5);

                    response.AveragePriceTarget.Should().Be(0);
                    response.HighestPriceTarget.Should().Be(0);
                    response.LowestPriceTarget.Should().Be(0);
                    response.PriceTargetPercentage.Should().Be(0);

                    response.StrongBuy.Should().Be(0);
                    response.Buy.Should().Be(0);
                    response.Hold.Should().Be(0);
                    response.Sell.Should().Be(0);
                    response.StrongSell.Should().Be(0);
                    response.AverageBrokerRecommendation.Should().Be(5);
                }
            }
        }
    }
}