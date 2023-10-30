using SWE.Time.Extensions;
using SWE.Time.Utilities;
using System.Text.Json.Serialization;

namespace BullMonitor.Abstractions.Responses
{
    public class TipRanksResponse
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        public string companyName { get; set; }
        public string companyFullName { get; set; }
        public string stockUid { get; set; }
        public int sectorID { get; set; }
        public string market { get; set; }
        public string description { get; set; }
        public bool hasEarnings { get; set; }
        public bool hasDividends { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool Found { get; set; } = true;

    public TipRanksStockScore tipRanksStockScore { get; set; }

        public List<Price> prices { get; set; }
        //public List<Consensus> consensuses { get; set; }
        //public List<PtConsensus> ptConsensus { get; set; }
        public List<ConsensusOverTime> consensusOverTime { get; set; }
        //public List<ConsensusOverTime> bestConsensusOverTime { get; set; }

        //public List<Expert> experts { get; set; }
        //public List<CorporateInsiderTransaction> corporateInsiderTransactions { get; set; }

        [JsonPropertyName("bloggerSentiment")]
        public BloggerSentiment BloggerSentiment { get; set; }
        //[JsonPropertyName("insidrConfidenceSignal")]
        //public InsiderConfidenceSignal InsiderConfidenceSignal { get; set; }
    }

    public class Price
    {
        public DateTime date { get; set; }
        public string d { get; set; }
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
        public decimal p { get; set; }
    }

    public class PriceTarget
    {
        public string consensus { get; set; }
        public string average { get; set; }
        public string high { get; set; }
        public string low { get; set; }
    }

    public class Analyst
    {
        public string name { get; set; }
        public string ticker { get; set; }
        public string action { get; set; }
        public string fromRating { get; set; }
        public string toRating { get; set; }
        public string priceTarget { get; set; }
        public string firm { get; set; }
        public DateTime date { get; set; }
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
        public string url { get; set; }
    }

    public class Consensus
    {
        private DateTimeOffset? _referenceDate = null;
        public int rating { get; set; }
        public int nB { get; set; }
        public int nH { get; set; }
        public int nS { get; set; }
        public int period { get; set; }
        public int bench { get; set; }
        public int mStars { get; set; }
        public string d { get; set; }
        public DateTimeOffset ReferenceDate => _referenceDate ??= DateTimeOffset
            .ParseExact(d, "d/M/yy", null)
            .SetToStartOfDay(TimeZoneInfoUtilities.DutchTimeZoneInfo);
        public int isLatest { get; set; }
        public decimal? priceTarget { get; set; }
    }

    public class PtConsensus
    {
        public int period { get; set; }
        public int bench { get; set; }
        public decimal? priceTarget { get; set; }
        public int? priceTargetCurrency { get; set; }
        public string? priceTargetCurrencyCode { get; set; }
        public decimal? high { get; set; }
        public decimal? low { get; set; }
    }

    public class TipRanksStockScore
    {
        public decimal? returnOnAssets { get; set; }
        public decimal? sixMonthsMomentum { get; set; }
        public decimal? volatilityLevel { get; set; }
        public int? score { get; set; }
        public decimal? returnOnEquity { get; set; }
        public int? volatilityLevelRating { get; set; }
        public decimal? twelveMonthsMomentum { get; set; }
        public decimal? simpleMovingAverage { get; set; }
        public decimal? assetGrowth { get; set; }
    }

    public class ConsensusOverTime
    {
        public int buy { get; set; }
        public int hold { get; set; }
        public int sell { get; set; }
        public DateTime date { get; set; }
        private DateTimeOffset? _referenceDate = null;
        public DateTimeOffset ReferenceDate => _referenceDate ??= date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);
        public int consensus { get; set; }
        public decimal? priceTarget { get; set; }
    }
    public class Expert
    {
        [JsonPropertyName("bloggerSentiment")]
        public string Name { get; set; }
        [JsonPropertyName("firm")]
        public string Firm { get; set; }
        [JsonPropertyName("eUid")]
        public string EUid { get; set; }
        [JsonPropertyName("eTypeId")]
        public int ETypeId { get; set; }
        [JsonPropertyName("expertImg")]
        public string ExpertImg { get; set; }
        [JsonPropertyName("ratings")]
        public List<Rating> Ratings { get; set; }
        [JsonPropertyName("stockSuccessRate")]
        public double? StockSuccessRate { get; set; }
        [JsonPropertyName("stockAverageReturn")]
        public double? StockAverageReturn { get; set; }
        [JsonPropertyName("stockTotalRecommendations")]
        public int? StockTotalRecommendations { get; set; }
        [JsonPropertyName("stockGoodRecommendations")]
        public int? StockGoodRecommendations { get; set; }
        [JsonPropertyName("rankings")]
        public List<Ranking> Rankings { get; set; }
        [JsonPropertyName("stockId")]
        public int StockId { get; set; }
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        [JsonPropertyName("newPictureUrl")]
        public string NewPictureUrl { get; set; }
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
        public string D { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("pos")]
        public int Pos { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("priceTarget")]
        public object PriceTarget { get; set; }

        [JsonPropertyName("convertedPriceTarget")]
        public object ConvertedPriceTarget { get; set; }

        [JsonPropertyName("quote")]
        public Quote Quote { get; set; }

        [JsonPropertyName("siteName")]
        public string SiteName { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

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
        public string Title { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        private DateTimeOffset? _referenceDate = null;

        [JsonIgnore]
        public DateTimeOffset ReferenceDate => _referenceDate ??= Date.ToDateTimeOffset(TimeZoneInfoUtilities.DutchTimeZoneInfo);

        //[JsonPropertyName("quote")]
        //public object Quote { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("siteName")]
        public string SiteName { get; set; }
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
        public string Bearish { get; set; }

        [JsonPropertyName("bullish")]
        public string Bullish { get; set; }

        [JsonPropertyName("bullishCount")]
        public int BullishCount { get; set; }

        [JsonPropertyName("bearishCount")]
        public int BearishCount { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("avg")]
        public decimal Avg { get; set; }

        [JsonPropertyName("neutral")]
        public string Neutral { get; set; }

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