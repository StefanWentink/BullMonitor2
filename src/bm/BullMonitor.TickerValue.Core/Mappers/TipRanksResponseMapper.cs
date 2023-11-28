using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using SWE.Extensions.Extensions;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Core.Mappers
{
    public class TipRanksResponseMapper
        : IMapper<
            (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
            IEnumerable<TipRanksEntity>>
    {
        public Task<IEnumerable<TipRanksEntity>> Map(
            (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly) value,
            CancellationToken cancellationToken)
        {
            var result = new List<TipRanksEntity>();

            var ticker = value.response.Ticker;
            var referenceDate = value.referenceDate;

            var dates = value.targetDateOnly
                ? value.referenceDate.Yield().ToList()
                : value
                    .response
                    .prices
                    .Select(price => price.ReferenceDate)
                    .Distinct();

            var responses = dates
                .Select(date =>
                {
                    var entity = new TipRanksEntity(value.response.Ticker)
                    {
                        ReferenceDate = date
                    };

                    var price = value
                        .response
                        .prices
                        .Where(x => x.ReferenceDate <= date)
                        .OrderByDescending(x => x.ReferenceDate)
                        .FirstOrDefault();

                    var priceValue = value == default
                        ? 0m
                        : price?.p ?? 0m;

                    var consensus = value
                        .response
                        .consensusOverTime
                        .Where(x => x.ReferenceDate <= date)
                        .OrderByDescending(x => x.ReferenceDate)
                        .FirstOrDefault();

                    BloggerSentiment? sentiment = (date == value.referenceDate)
                        ? value
                            .response
                            .BloggerSentiment
                        : null;

                    var tipRanksValue = new TipRanksValue(
                        priceValue,
                        consensus?.buy ?? 0,
                        consensus?.hold ?? 0,
                        consensus?.sell ?? 0,
                        consensus?.consensus ?? 0,
                        consensus?.priceTarget ?? 0,
                        sentiment?.BullishCount ?? 0,
                        sentiment?.NeutralCount ?? 0,
                        sentiment?.BearishCount ?? 0,
                        sentiment?.Score ?? 0,
                        sentiment?.Avg ?? 0);

                    return new TipRanksEntity(value.response.Ticker)
                    {
                        ReferenceDate = date,
                        Value = tipRanksValue
                    };

                }).ToList();

            return Task.FromResult<IEnumerable<TipRanksEntity>>(responses);
        }
    }
}
