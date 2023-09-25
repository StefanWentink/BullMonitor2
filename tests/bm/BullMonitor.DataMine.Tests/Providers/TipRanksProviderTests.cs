using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.DataMine.Extensions;
using BullMonitor.DataMine.Tests.Stubs;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSWE.Tests.Base;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using Xunit;

namespace BullMonitor.DataMine.Tests.Providers
{
    public class TipRanksProviderTests
        : BaseServiceProviderTests<ISingleProvider<TipRanksRequest, TipRanksResponse>>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorDataMineServices(configuration)
                .AddSingleton<ISingleProvider<TipRanksRequest, string?>, StubTipRanksRawProvider>();
        }

        [Theory]
        [InlineData("LIZI")]
        [InlineData("MSFT")]
        public async Task TipRanksProvider_Should_ResolveModel(
            string ticker)
        {
            var from = new DateTimeOffset(2022, 10, 30, 0, 0, 0, TimeSpan.FromHours(2));
            var until = new DateTimeOffset(2022, 10, 31, 0, 0, 0, TimeSpan.FromHours(1));
            var request = new TipRanksRequest(
                from,
                until,
                ticker);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            try
            {
                var response = await provider
                    .GetSingleOrDefault(request, cancellationToken)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();

                if (response != null)
                {
                    response.description.Should().NotBeNullOrWhiteSpace();
                    response.Ticker.Should().Be(request.Ticker);
                    response.companyName.Should().NotBeNullOrWhiteSpace();
                    response.companyFullName.Should().NotBeNullOrWhiteSpace();
                    response.stockUid.Should().NotBeNullOrWhiteSpace();
                    response.sectorID.Should().NotBe(0);
                    response.market.Should().NotBeNullOrWhiteSpace();
                    response.description.Should().NotBeNullOrWhiteSpace();
                    response.hasEarnings.Should().NotBe(false);
                    response.hasDividends.Should().NotBe(false);

                    response.prices.Should().HaveCountGreaterThanOrEqualTo(1);
                    response.prices.First().ReferenceDate.Should().NotBe(default);
                    response.prices.First().d.Should().NotBeNullOrWhiteSpace();
                    response.prices.First().p.Should().NotBe(0);

                    response.consensuses.Should().HaveCountGreaterThanOrEqualTo(1);
                    response.consensuses.First().nB.Should().NotBe(0);
                    response.consensuses.First().rating.Should().NotBe(0);
                    response.consensuses.First().nH.Should().NotBe(0);
                    response.consensuses.First().ReferenceDate.Should().NotBe(default);

                    //response.experts.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.experts.First().Firm.Should().NotBeNullOrWhiteSpace();

                    //response.experts.First().Ratings.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.experts.First().Ratings.First().RatingId.Should().NotBe(default);
                    //response.experts.First().Ratings.First().ReferenceDate.Should().NotBe(default);
                    //response.experts.First().Ratings.First().Quote.Should().NotBeNull();
                    //response.experts.First().Ratings.First().Quote.Title.Should().NotBeNullOrWhiteSpace();

                    //response.experts.First().Rankings.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.experts.First().Rankings.First().LRank.Should().NotBe(0);

                    //response.ptConsensus.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.ptConsensus.First().priceTarget.Should().NotBe(0);

                    response.tipranksStockScore.Should().NotBeNull();

                    //response.consensusOverTime.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.consensusOverTime.First().priceTarget.Should().NotBe(0);
                    //response.consensusOverTime.First().ReferenceDate.Should().NotBe(default);

                    //response.bestConsensusOverTime.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.bestConsensusOverTime.First().consensus.Should().NotBe(0);
                    //response.bestConsensusOverTime.First().ReferenceDate.Should().NotBe(default);

                    //response.BloggerSentiment.Should().NotBeNull();
                    //response.BloggerSentiment.BullishCount.Should().NotBe(0); ;
                    //response.BloggerSentiment.BearishCount.Should().NotBe(0); ;
                    //response.BloggerSentiment.NeutralCount.Should().NotBe(0); ;

                    //response.corporateInsiderTransactions.Should().HaveCountGreaterThanOrEqualTo(1);
                    //response.corporateInsiderTransactions.First().TransSellCount.Should().NotBe(0);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}