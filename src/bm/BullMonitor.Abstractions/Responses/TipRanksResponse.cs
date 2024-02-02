using SWE.Time.Extensions;
using SWE.Time.Utilities;
using System.Text.Json.Serialization;

namespace BullMonitor.Abstractions.Responses
{
    public class TipRanksResponse
    {
        [JsonPropertyName("ticker")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Ticker { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string companyName { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string companyFullName { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string stockUid { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public int sectorID { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string market { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string description { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public bool hasEarnings { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public bool hasDividends { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Found { get; set; } = true;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
    public TipRanksStockScore tipRanksStockScore { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public List<Price> prices { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //public List<Consensus> consensuses { get; set; }
        //public List<PtConsensus> ptConsensus { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public List<ConsensusOverTime> consensusOverTime { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //public List<ConsensusOverTime> bestConsensusOverTime { get; set; }

        //public List<Expert> experts { get; set; }
        //public List<CorporateInsiderTransaction> corporateInsiderTransactions { get; set; }

        [JsonPropertyName("bloggerSentiment")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BloggerSentiment BloggerSentiment { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        //[JsonPropertyName("insidrConfidenceSignal")]
        //public InsiderConfidenceSignal InsiderConfidenceSignal { get; set; }
    }

    public class Price
    {
#pragma warning disable IDE1006 // Naming Styles
        public DateTime date { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string d { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
#pragma warning disable IDE1006 // Naming Styles
        public decimal p { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class PriceTarget
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string consensus { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string average { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string high { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string low { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public class Analyst
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string name { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string ticker { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string action { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string fromRating { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string toRating { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string priceTarget { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string firm { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public DateTime date { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string url { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public class Consensus
    {
        private DateTimeOffset? _referenceDate = null;
#pragma warning disable IDE1006 // Naming Styles
        public int rating { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int nB { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int nH { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int nS { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int period { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int bench { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int mStars { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
        public string d { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTimeOffset ReferenceDate => _referenceDate ??= DateTimeOffset
            .ParseExact(d, "d/M/yy", null)
            .SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo);
#pragma warning disable IDE1006 // Naming Styles
        public int isLatest { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? priceTarget { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class PtConsensus
    {
#pragma warning disable IDE1006 // Naming Styles
        public int period { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int bench { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? priceTarget { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int? priceTargetCurrency { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public string? priceTargetCurrencyCode { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? high { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? low { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class TipRanksStockScore
    {
#pragma warning disable IDE1006 // Naming Styles
        public decimal? returnOnAssets { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? sixMonthsMomentum { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? volatilityLevel { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int? score { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? returnOnEquity { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int? volatilityLevelRating { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? twelveMonthsMomentum { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? simpleMovingAverage { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? assetGrowth { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class ConsensusOverTime
    {
#pragma warning disable IDE1006 // Naming Styles
        public int buy { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int hold { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public int sell { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public DateTime date { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
#pragma warning disable IDE1006 // Naming Styles
        public int consensus { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public decimal? priceTarget { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
    public class Expert
    {
        [JsonPropertyName("bloggerSentiment")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("firm")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Firm { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("eUid")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string EUid { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("eTypeId")]
        public int ETypeId { get; set; }
        [JsonPropertyName("expertImg")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ExpertImg { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("ratings")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public List<Rating> Ratings { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("stockSuccessRate")]
        public double? StockSuccessRate { get; set; }
        [JsonPropertyName("stockAverageReturn")]
        public double? StockAverageReturn { get; set; }
        [JsonPropertyName("stockTotalRecommendations")]
        public int? StockTotalRecommendations { get; set; }
        [JsonPropertyName("stockGoodRecommendations")]
        public int? StockGoodRecommendations { get; set; }
        [JsonPropertyName("rankings")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public List<Ranking> Rankings { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("stockId")]
        public int StockId { get; set; }
        [JsonPropertyName("ticker")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Ticker { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("companyName")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string CompanyName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("newPictureUrl")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string NewPictureUrl { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonPropertyName("includedInConsensus")]
        public bool IncludedInConsensus { get; set; }
    }

    public class Rating
    {
        [JsonPropertyName("ratingId")]
        public int RatingId { get; set; }

        [JsonPropertyName("actionId")]
        public int ActionId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("d")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string D { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("url")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Url { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("pos")]
        public int Pos { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("priceTarget")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public object PriceTarget { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("convertedPriceTarget")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public object ConvertedPriceTarget { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("quote")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Quote Quote { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("siteName")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SiteName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("site")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Site { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("rd")]
        public DateTime RD { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= Timestamp.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
        //public object PriceTargetCurrency { get; set; }
        //public object ConvertedPriceTargetCurrency { get; set; }
        //public object ConvertedPriceTargetCurrencyCode { get; set; }
        //public object PriceTargetCurrencyCode { get; set; }
    }

    public class Quote
    {
        [JsonPropertyName("title")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Title { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        private DateTimeOffset? _referenceDate = null;

        [JsonIgnore]
        public DateTimeOffset ReferenceDate => _referenceDate ??= Date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);

        //[JsonPropertyName("quote")]
        //public object Quote { get; set; }

        [JsonPropertyName("site")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Site { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("link")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Link { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("siteName")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SiteName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public class Ranking
    {
        [JsonPropertyName("period")]
        public int Period { get; set; }

        [JsonPropertyName("bench")]
        public int Bench { get; set; }

        [JsonPropertyName("lRank")]
        public int LRank { get; set; }

        [JsonPropertyName("gRank")]
        public int GRank { get; set; }

        [JsonPropertyName("gRecs")]
        public int GRecs { get; set; }

        [JsonPropertyName("tRecs")]
        public int TRecs { get; set; }

        [JsonPropertyName("avgReturn")]
        public double AvgReturn { get; set; }

        [JsonPropertyName("stars")]
        public double Stars { get; set; }

        [JsonPropertyName("originalStars")]
        public double OriginalStars { get; set; }

        [JsonPropertyName("tPos")]
        public double TPos { get; set; }
    }

    public class BloggerSentiment
    {
        [JsonPropertyName("bearish")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Bearish { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("bullish")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Bullish { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("bullishCount")]
        public int BullishCount { get; set; }

        [JsonPropertyName("bearishCount")]
        public int BearishCount { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("avg")]
        public decimal Avg { get; set; }

        [JsonPropertyName("neutral")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Neutral { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [JsonPropertyName("neutralCount")]
        public int NeutralCount { get; set; }
    }


    public class InsiderConfidenceSignal
    {
        [JsonPropertyName("stockScore")]
        public double StockScore { get; set; }

        [JsonPropertyName("sectorScore")]
        public double SectorScore { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }

    public class CorporateInsiderTransaction
    {
        [JsonPropertyName("sharesBought")]
        public int? SharesBought { get; set; }

        [JsonPropertyName("insidersBuyCount")]
        public int InsidersBuyCount { get; set; }

        [JsonPropertyName("sharesSold")]
        public int? SharesSold { get; set; }

        [JsonPropertyName("insidersSellCount")]
        public int InsidersSellCount { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("transBuyCount")]
        public int TransBuyCount { get; set; }

        [JsonPropertyName("transSellCount")]
        public int TransSellCount { get; set; }

        [JsonPropertyName("transBuyAmount")]
        public double? TransBuyAmount { get; set; }

        [JsonPropertyName("transSellAmount")]
        public double? TransSellAmount { get; set; }

        [JsonPropertyName("informativeBuyCount")]
        public int InformativeBuyCount { get; set; }

        [JsonPropertyName("informativeSellCount")]
        public int InformativeSellCount { get; set; }

        [JsonPropertyName("informativeBuyAmount")]
        public double InformativeBuyAmount { get; set; }

        [JsonPropertyName("informativeSellAmount")]
        public double InformativeSellAmount { get; set; }
    }
}