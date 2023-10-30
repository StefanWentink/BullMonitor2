using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using SWE.Time.Extensions;

namespace BullMonitor.Data.Storage.Models
{
    public class TipRanksEntity
    {
        private DateTimeOffset? _referenceDate = null;

        public string Ticker { get; set; }

        public TipRanksEntity(string ticker)
        {
            Ticker = ticker;
        }

        public TipRanksValue Value { get; set; } = new TipRanksValue(
            0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0);

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

    public class TipRanksValue
    {
        public TipRanksValue(
            decimal price,

            int buy,
            int hold,
            int sell,
            int consensus,
            decimal priceTarget,

            int bullishSentiments,
            int neutralSentiments,
            int bearishSentiments,

            int sentimentsScore,
            decimal averageSentiments)
        {
            Price = price;

            Buy = buy;
            Hold = hold;
            Sell = sell;
            Consensus = consensus;
            PriceTarget = priceTarget;

            BullishSentiments = bullishSentiments;
            NeutralSentiments = neutralSentiments;
            BearishSentiments = bearishSentiments;

            SentimentsScore = sentimentsScore;
            AverageSentiments = averageSentiments;
    }

        public decimal Price { get; set; }

        //public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int Consensus { get; set; }
        public decimal PriceTarget { get; set; }

        public int BullishSentiments { get; set; }
        public int NeutralSentiments { get; set; }
        public int BearishSentiments { get; set; }

        public int SentimentsScore { get; set; }
        public decimal AverageSentiments { get; set; }
    }

    public class TipRanksMeta
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
    }
}