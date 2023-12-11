using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Tests.Base;
using Xunit;

namespace BullMonitor.Ticker.Core.Tests
{
    public class ValueProviderTests
        : BaseServiceProviderTests<IValueProvider>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorTickerCoreServices(configuration);
        }

        [Fact()]
        public async Task ValueProvider_Should_Create()
        {
            try
            {
                var provider = Create();

                provider.Should().NotBeNull();
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
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            try
            {
                var response = await provider
                    .GetSingleOrDefault(
                        ticker,
                        cancellationToken)
                    .ConfigureAwait(false);

                response.Should().NotBeNull();
                response?.Values.Should().NotBeEmpty();
            }
            catch (Exception exception)
            {
                Skip.If(exception != null);
            }
        }
    }
}
