using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using SWE.Time.Extensions;

namespace BullMonitor.Data.Storage.Models
{
    //public record struct ZacksRankEntity(
    //        Guid TickerId,
    //        DateTimeOffset ReferenceDate,
    //        int Rank,
    //        int Value,
    //        int Growth,
    //        int Momentum,
    //        int VGM)
    //    : ITickerValue
    //{ }

    public class ZacksRankEntity
    {
        private DateTimeOffset? _referenceDate = null;

        //[BsonGuidRepresentation(GuidRepresentation.Standard)]
        //public Guid Id { get; set; }
        public string Ticker { get; set; }

        public ZacksRankEntity(string ticker)
        {
            Ticker = ticker;
        }

        public ICollection<ZacksRankValue> Values { get; set; } = new HashSet<ZacksRankValue>();

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
        //[BsonIgnoreIfNull]
        public int Rank { get; set; }
        //[BsonIgnoreIfNull]
        public int Value { get; set; }
        //[BsonIgnoreIfNull]
        public int Growth { get; set; }
        //[BsonIgnoreIfNull]
        public int Momentum { get; set; }
        //[BsonIgnoreIfNull]
        public int VGM { get; set; }
    }

    public class ZacksRankMeta
    {
        public Guid Id { get; set; }
        //[BsonIgnoreIfNull]
        public string Ticker { get; set; }
    }
}