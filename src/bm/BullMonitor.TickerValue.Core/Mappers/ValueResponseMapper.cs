using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.TickerValue.Core.Mappers
{
    public class ValueResponseMapper
        : IMapper<
            (IEnumerable<ZacksEntity> zacks, IEnumerable<TipRanksEntity> tipranks),
            IDictionary<DateTimeOffset, ValueResponse>>
    {
        protected IMapper<ZacksEntity?, ZacksValueResponse> ZacksValueResponseMapper { get; }
        protected IMapper<TipRanksEntity?, TipRanksValueResponse> TipRanksValueResponseMapper { get; }

        public ValueResponseMapper(
            IMapper<ZacksEntity?, ZacksValueResponse> zacksValueResponseMapper,
            IMapper<TipRanksEntity?, TipRanksValueResponse> tipRanksValueResponseMapper)
        {
            ZacksValueResponseMapper = zacksValueResponseMapper;
            TipRanksValueResponseMapper = tipRanksValueResponseMapper;
        }

        public async Task<IDictionary<DateTimeOffset, ValueResponse>> Map(
            (IEnumerable<ZacksEntity> zacks, IEnumerable<TipRanksEntity> tipranks) value,
            CancellationToken cancellationToken)
        {
            var dates = value.zacks.Select(x => x.ReferenceDate).ToList();
            dates.AddRange(value.tipranks.Select(x => x.ReferenceDate));

            var tasks = dates
                .Distinct()
                .Select(async date =>
                {
                    var tipRanksEntity = value.tipranks.FirstOrDefault(x => x.ReferenceDate.Equals(date));
                    var zacksEntity = value.zacks.FirstOrDefault(x => x.ReferenceDate.Equals(date));
                    var ticker = zacksEntity?.Ticker ?? tipRanksEntity?.Ticker ?? string.Empty;

                    var tipRanksValueResponse = await TipRanksValueResponseMapper.Map(tipRanksEntity, cancellationToken).ConfigureAwait(false);
                    var zacksValueResponse = await ZacksValueResponseMapper.Map(zacksEntity, cancellationToken).ConfigureAwait(false);

                    return (date, value: new ValueResponse(ticker, tipRanksValueResponse, zacksValueResponse));
                });

            var result = await Task.WhenAll(tasks);

            return result.ToDictionary(x => x.date, x => x.value);
        }
    }
}
