using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using SWE.Time.Extensions;

namespace BullMonitor.Data.Storage.Models
{
    public class ZacksRankEntity
    {
        private DateTimeOffset? _referenceDate = null;

        public string Ticker { get; set; }

        public ZacksRankEntity(string ticker)
        {
            Ticker = ticker;
        }

        public ZacksRankValue Value { get; set; } = new ZacksRankValue(
            3, 3, 3, 3, 3,
            0, 0, 0, 0,
            0, 0, 0, 0, 0, 0);

        /// <summary>
        /// Is the Presentation field... use <see cref="ReferenceDateDb"/> for db filtering.
        /// </summary>
        [BsonIgnore]
        public DateTimeOffset ReferenceDate
        {
            get => _referenceDate ??= ReferenceDateDb.ToDateTimeOffsetUtc();
            set
            {
                _referenceDate = null;
                ReferenceDateDb = value.ToUtcTimeZone().DateTime;
            }
        }

        /// <summary>
        /// Is the DB field... this is used for filtering
        /// </summary>
        [JsonPropertyName(nameof(ReferenceDate))]
        [BsonElement(nameof(ReferenceDate))]
        public DateTime ReferenceDateDb { get; set; }
    }

    public class ZacksRankValue
    {
        public ZacksRankValue(
            int rank,
            int value,
            int growth,
            int momentum,
            int vGM,

            decimal averagePriceTarget,
            decimal lowestPriceTarget,
            decimal highestPriceTarget,
            decimal percentagePriceTarget,

            int strongBuy,
            int buy,
            int hold,
            int sell,
            int strongSell,

            decimal averageBrokerRecommendation)
        {
            Rank = rank;
            Value = value;
            Growth = growth;
            Momentum = momentum;
            VGM = vGM;

            AveragePriceTarget = averagePriceTarget;
            LowestPriceTarget = lowestPriceTarget;
            HighestPriceTarget = highestPriceTarget;
            PercentagePriceTarget = percentagePriceTarget;

            StrongBuy = strongBuy;
            Buy = buy;
            Hold = hold;
            Sell = sell;
            StrongSell = strongSell;

            AverageBrokerRecommendation = averageBrokerRecommendation;
        }

        public int Rank { get; set; }
        public int Value { get; set; }
        public int Growth { get; set; }
        public int Momentum { get; set; }
        public int VGM { get; set; }

        public decimal AveragePriceTarget { get; set; }
        public decimal LowestPriceTarget { get; set; }
        public decimal HighestPriceTarget { get; set; }
        public decimal PercentagePriceTarget { get; set; }


        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }

        public decimal AverageBrokerRecommendation { get; set; }
    }

    public class ZacksRankMeta
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
    }
}