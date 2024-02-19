using BullMonitor.Ticker.Api.Abstractions.Responses;
using FluentAssertions;
using SWE.Infrastructure.Abstractions.Factories;
using System.Text.Json;
using Xunit;

namespace BullMonitor.Ticker.Api.Abstractions.Tests
{
    public class CompanyResponseSerializationTests
    {
        private const string _json = "{\"id\":\"d1213670-af23-461e-bcf4-08dbb84d9919\",\"code\":\"MSFT\",\"name\":\"Microsoft Corporation Common Stock\",\"currencyId\":\"1c37ae84-7878-4af0-fc9a-08dbb84c7b54\",\"exchangeId\":\"6c9465a0-aa4a-4d5f-319e-08dbb84c9b94\",\"industryId\":\"2d1e1a73-7a18-400c-ae9e-08dbb84d98fd\"}";

        private static readonly TipRanksValueResponse _lastTipRanksValueResponse = new TipRanksValueResponse(
            98,
            10, 11, 12, 13, 14,
            24, 25, 26, 27, 28);

        private static readonly TipRanksValueResponse _firstTipRanksValueResponse = new TipRanksValueResponse(
            99,
            11, 12, 13, 14, 15,
            25, 26, 27, 28, 29);

        private static readonly ZacksValueResponse _lastZacksValueResponse = new ZacksValueResponse(
            5, 4, 3, 2, 1,
            65, 60, 75, 56,
            51, 52, 53, 54, 55, 3.6m);

        private static readonly ZacksValueResponse _firstZacksValueResponse = new ZacksValueResponse(
            1, 2, 3, 4, 5,
            35, 30, 45, 6,
            41, 42, 43, 44, 45, 2.4m);

        private static ValueResponse _lastValueResponse = new ValueResponse(
            "SWE",
            _lastTipRanksValueResponse,
            _lastZacksValueResponse);

        private static ValueResponse _firstValueResponse = new ValueResponse(
            "SWE",
            _firstTipRanksValueResponse,
            _firstZacksValueResponse);

        private static CompanyResponse _response = new CompanyResponse(
            Guid.Parse("d1213670-af23-461e-bcf4-08dbb84d9919"),
            "MSFT",
            new Dictionary<DateTimeOffset, ValueResponse>
                {
                    {
                        new DateTimeOffset(2023, 11, 30, 0, 0, 0, TimeSpan.FromHours(1)),
                        _lastValueResponse
                    },
                    {
                        new DateTimeOffset(2023, 11, 29, 0, 0, 0, TimeSpan.FromHours(2)),
                        _firstValueResponse
                    }
                }
            );

        [Fact]
        public void CompanyResponse_Should_Serialize()
        {
            var actual = JsonSerializer
                .Serialize(_response, JsonSerializerOptionsFactory.Options);

            actual.Should().BeEquivalentTo(_json);
        }

        [Fact]
        public void CompanyResponse_Should_Deserialize()
        {
            var actual = JsonSerializer
                .Deserialize<CompanyResponse>(_json, JsonSerializerOptionsFactory.Options);

            ArgumentNullException.ThrowIfNull(actual);

            actual.Id.Should().Be(_response.Id);
            actual.Code.Should().Be(_response.Code);

            var actualDictionary = actual.Values;
            var responseDictionary = _response.Values;

            actualDictionary.Should().HaveCount(2);

            var firstActual = actualDictionary
                .OrderBy(x => x.Key)
                .First();

            var lastActual = actualDictionary
                .OrderBy(x => x.Key)
                .Last();


            firstActual.Equals(_firstValueResponse).Should().BeTrue();
            lastActual.Equals(_lastValueResponse).Should().BeTrue();
        }
    }
}