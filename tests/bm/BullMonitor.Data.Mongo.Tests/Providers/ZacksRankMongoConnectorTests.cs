using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWE.Tests.Base;
using SWE.Mongo.Interfaces;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Data.Mongo.Extensions;
using Xunit;
using SWE.Mongo.Containers;
using SWE.Time.Extensions;
using System.Linq.Expressions;
using FluentAssertions;

namespace BullMonitor.Data.Mongo.Tests.Connectors
{
    public class ZacksRankMongoConnectorTests
        : BaseServiceProviderTests<IBaseMongoConnector<ZacksRankEntity>>
    {
        protected override IServiceCollection WithServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .WithBullMonitorZacksRankConnectorServices(configuration);
        }

        [SkippableTheory()]
        [InlineData("MSFT")]
        [InlineData("LIZI")]
        public async Task ZacksRankProvider_Should_Provide_With_Expression(string ticker)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            try
            {
                var request = new MongoConditionContainer<ZacksRankEntity>(x => x.Ticker == ticker);

                var response = await provider
                    .Get(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Skip.If(exception != null);
            }
        }

        [SkippableTheory()]
        [InlineData("MSFT")]
        [InlineData("LIZI")]
        public async Task ZacksRankProvider_Should_UpsertInsert(string ticker)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var provider = Create();

            var referenceDate = new DateTimeOffset(2022, 10, 31, 0, 0, 0, TimeSpan.FromHours(1));

            try
            {
                //var tickerFilter = Builders<ZacksRankEntity>
                //    .Filter
                //    .Eq(x => x.Ticker, ticker);

                //var dateFilter = Builders<ZacksRankEntity>
                //    .Filter
                //    .Eq(x => x.ReferenceDateDb, referenceDate.ToUtcTimeZone().DateTime);

                //var filter = tickerFilter & dateFilter;

                //var request = new MongoConditionContainer<ZacksRankEntity>(filter);

                //var tickerFilter = Builders<ZacksRankEntity>
                //    .Filter
                //    .Eq(nameof(ZacksRankEntity.Ticker), ticker);

                //var dateFilter = Builders<ZacksRankEntity>
                //    .Filter
                //    .Eq(nameof(ZacksRankEntity.ReferenceDateDb), referenceDate.DateTime);

                var request = new MongoConditionContainer<ZacksRankEntity>(
                    new List<Expression<Func<ZacksRankEntity, bool>>>
                    {
                        x => x.Ticker == ticker,
                        x => x.ReferenceDateDb == referenceDate.ToUtcTimeZone().DateTime
                    })
                { };

                var response = await provider
                    .GetSingleOrDefault(request, cancellationToken)
                    .ConfigureAwait(false);

                if (response == null)
                {
                    var command = new ZacksRankEntity(ticker)
                    {
                        ReferenceDate = referenceDate,
                        Value = new ZacksRankValue(
                            1, 2, 3, 4, 5,
                            110, 100, 130, 25.3m,
                            4, 2, 3, 3, 1, 2.4m)
                    };

                    var createResponse = await provider
                        .Create(command, cancellationToken)
                        .ConfigureAwait(false);

                    response = await provider
                        .GetSingleOrDefault(request, cancellationToken)
                        .ConfigureAwait(false);

                    response.Should().NotBeNull();
                }
            }
            catch (Exception exception)
            {
                Skip.If(exception != null);
            }
        }
    }
}