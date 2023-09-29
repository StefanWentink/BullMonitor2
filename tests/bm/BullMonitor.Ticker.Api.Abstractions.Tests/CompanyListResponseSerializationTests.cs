using BullMonitor.Ticker.Api.Abstractions.Responses;
using FluentAssertions;
using SWE.Infrastructure.Abstractions.Factories;
using System.Text.Json;
using Xunit;

namespace BullMonitor.Ticker.Api.Abstractions.Tests
{
    public class CompanyListResponseSerializationTests
    {
        private const string _json = "{\"id\":\"d1213670-af23-461e-bcf4-08dbb84d9919\",\"code\":\"MSFT\",\"name\":\"Microsoft Corporation Common Stock\",\"currencyId\":\"1c37ae84-7878-4af0-fc9a-08dbb84c7b54\",\"exchangeId\":\"6c9465a0-aa4a-4d5f-319e-08dbb84c9b94\",\"industryId\":\"2d1e1a73-7a18-400c-ae9e-08dbb84d98fd\"}";

        private static CompanyListResponse _response = new CompanyListResponse(
            Guid.Parse("d1213670-af23-461e-bcf4-08dbb84d9919"),
            "MSFT",
            "Microsoft Corporation Common Stock",
            Guid.Parse("1c37ae84-7878-4af0-fc9a-08dbb84c7b54"),
            Guid.Parse("6c9465a0-aa4a-4d5f-319e-08dbb84c9b94"),
            Guid.Parse("2d1e1a73-7a18-400c-ae9e-08dbb84d98fd"));

        [Fact]
        public void CompanyListResponse_Should_Serialize()
        {
            var actual = JsonSerializer
                .Serialize(_response, JsonSerializerOptionsFactory.Options);

            actual.Should().BeEquivalentTo(_json);
        }

        [Fact]
        public void CompanyListResponse_Should_Deserialize()
        {
            var actual = JsonSerializer
                .Deserialize<CompanyListResponse>(_json, JsonSerializerOptionsFactory.Options);

            actual.Id.Should().Be(_response.Id);
            actual.Code.Should().Be(_response.Code);
            actual.Name.Should().Be(_response.Name);
            actual.CurrencyId.Should().Be(_response.CurrencyId);
            actual.ExchangeId.Should().Be(_response.ExchangeId);
            actual.IndustryId.Should().Be(_response.IndustryId);
        }
    }
}
