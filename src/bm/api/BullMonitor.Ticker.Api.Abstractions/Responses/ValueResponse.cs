namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record ValueResponse(
            string Ticker,
            TipRanksValueResponse TipRanks,
            ZacksValueResponse Zacks)
        : IEquatable<ValueResponse>
    {
        public ValueResponse(
            string Ticker,
            TipRanksValueResponse TipRanks)
            : this(
                Ticker,
                TipRanks,
                new ZacksValueResponse())
        { }

        public ValueResponse(
            string Ticker,
             ZacksValueResponse Zacks)
            : this(
                Ticker,
                new TipRanksValueResponse(),
                Zacks)
        { }
    }
}