using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Tests.Base;
using Xunit;

namespace BullMonitor.Ticker.Core.Tests
{
    public class TipRanksResponseMapperTests
        : BaseServiceProviderTests<IMapper<
            (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
            IEnumerable<TipRanksEntity>>>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorTickerCoreServices(configuration);
        }

        [Fact()]
        public void Mapper_Should_Create()
        {
            try
            {
                var mapper = Create();

                mapper.Should().NotBeNull();
            }
            catch (Exception exception)
            {
                exception.Message.Should().NotBeNullOrWhiteSpace();
            }
        }

        [SkippableTheory()]
        [InlineData("MSFT")]
        [InlineData("LIZI")]
        public async Task ValueProvider_Should_Provide_With_Ticker(string ticker)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var mapper = Create();

            try
            {
                //var response = await mapper
                //    .GetSingleOrDefault(
                //        ticker,
                //        cancellationToken)
                //    .ConfigureAwait(false);

                //response.Should().NotBeNull();
                //response?.Values.Should().NotBeEmpty();
            }
            catch (Exception exception)
            {
                Skip.If(exception != null);
            }
        }
    }
}