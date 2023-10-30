using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace BullMonitor.DataMine.Providers
{
    public class ZacksProvider
        : ISingleProvider<ZacksRequest, ZacksResponse>
    {        // Set the URL
        protected ISingleProvider<ZacksRequest, string?> RawProvider { get; }
        protected ILogger<ZacksProvider> Logger { get; }

        public ZacksProvider(
            ISingleProvider<ZacksRequest, string?> rawProvider,
            ILogger<ZacksProvider> logger)
        {
            RawProvider = rawProvider;
            Logger = logger;
        }

        public async Task<ZacksResponse?> GetSingleOrDefault(
            ZacksRequest value,
            CancellationToken cancellationToken)
        {
            // Read the response content
            string? responseString = await RawProvider
                .GetSingleOrDefault(value, cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(responseString))
            {
                return new ZacksResponse(value.Ticker, false);
            }

            var rank = 0;
            var valueString = "F";
            var growthString = "F";
            var momentumString = "F";
            var vgmString = "F";

            var averagePriceTargetString = "$0";
            var highestPriceTargetString = "$0";
            var lowestPriceTargetString = "$0";
            var priceTargetPercentageString = "0%";

            var strongBuyString = "0";
            var buyString = "0";
            var holdString = "0";
            var sellString = "0";
            var strongSellString = "0";
            var averageBrokerRecommendationString = "5.00";

            // Load the HTML into an HtmlDocument
            HtmlDocument document = new();
            document.LoadHtml(responseString);

            var rankViewFilter = "//p[@class='rank_view']";

            try
            {
                var rankNodes = document
                    .DocumentNode
                    .SelectNodes(rankViewFilter)
                    .ToList();

                var rankNode = rankNodes
                    .Where(x =>
                        x.HasChildNodes
                        && (
                            x.FirstChild.InnerText.Contains(" 1-")
                            || x.FirstChild.InnerText.Contains(" 2-")
                            || x.FirstChild.InnerText.Contains(" 3-")
                            || x.FirstChild.InnerText.Contains(" 4-")
                            || x.FirstChild.InnerText.Contains(" 5-")
                        )).SingleOrDefault();

                if (rankNode == null)
                {
                    return new ZacksResponse(value.Ticker);
                }

                if (rankNode != null)
                {
                    var priceTargetFilter = "//div[@class='key-expected-earnings-data-module price-targets']";

                    try
                    {
                        var priceNode = document
                            .DocumentNode
                            .SelectNodes(priceTargetFilter)
                            .SingleOrDefault();

                        if (priceNode != null)
                        {
                            var table = priceNode
                                .ChildNodes
                                .Where(x => x.Name.Contains("table", StringComparison.OrdinalIgnoreCase))
                                .SingleOrDefault();

                            if (table != null)
                            {
                                var body = table
                                    .ChildNodes
                                    .Where(x => x.Name.Contains("tbody", StringComparison.OrdinalIgnoreCase))
                                    .SingleOrDefault();

                                if (body != null)
                                {
                                    var row = body
                                        .ChildNodes
                                        .Where(x => x.Name.Contains("tr", StringComparison.OrdinalIgnoreCase))
                                        .SingleOrDefault();

                                    if (row != null)
                                    {
                                        var fields = row
                                            .ChildNodes
                                            .Where(x => !x.Name.Contains("#text", StringComparison.OrdinalIgnoreCase))
                                            .ToList();

                                        if (fields.Count == 4)
                                        {
                                            averagePriceTargetString = fields.First().InnerText;
                                            highestPriceTargetString = fields.Skip(1).First().InnerText;
                                            lowestPriceTargetString = fields.Skip(2).First().InnerText;
                                            priceTargetPercentageString = fields.Skip(3).First().InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.LogError(exception, exception.Message);
                    }

                    var recommendationFilter = "//section[@id='quote_brokerage_recomm']";

                    try
                    {
                        var recommendationNode = document
                            .DocumentNode
                            .SelectNodes(recommendationFilter)
                            .SingleOrDefault();

                        if (recommendationNode != null)
                        {
                            var table = recommendationNode
                                .ChildNodes
                                .Where(x => x.Name.Contains("table", StringComparison.OrdinalIgnoreCase))
                                .SingleOrDefault();

                            if (table != null)
                            {
                                var body = table
                                    .ChildNodes
                                    .Where(x => x.Name.Contains("tbody", StringComparison.OrdinalIgnoreCase))
                                    .SingleOrDefault();

                                if (body != null)
                                {
                                    var rows = body
                                        .ChildNodes
                                        .Where(x => x.Name.Contains("tr", StringComparison.OrdinalIgnoreCase))
                                        .ToList();

                                    if ((rows?.Count ?? 0) == 6)
                                    {
                                        strongBuyString = rows.Skip(0).First().ChildNodes.Skip(3).First().InnerText;
                                        buyString = rows.Skip(1).First().ChildNodes.Skip(3).First().InnerText;
                                        holdString = rows.Skip(2).First().ChildNodes.Skip(3).First().InnerText;
                                        sellString = rows.Skip(3).First().ChildNodes.Skip(3).First().InnerText;
                                        strongSellString = rows.Skip(4).First().ChildNodes.Skip(3).First().InnerText;
                                        averageBrokerRecommendationString = rows.Skip(5).First().ChildNodes.Skip(3).First().InnerText;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.LogError(exception, exception.Message);
                    }

                    var rankString = rankNode
                        .InnerText
                        .Split("-")
                        .First();

                    if (!int.TryParse(rankString, out rank))
                    {
                        Logger.LogError($"Could not determine {nameof(rankString)} from '{rankString}'.");
                        return new ZacksResponse(value.Ticker);
                    }

                    var valueRankNode = rankNodes
                        .Where(x => x.OuterHtml.Contains("<span class=\"composite_val"))
                        .SingleOrDefault();

                    if (valueRankNode != null)
                    {
                        HtmlDocument documentValueRankNode = new();
                        documentValueRankNode.LoadHtml(valueRankNode.OuterHtml);

                        var letters = valueRankNode
                            .InnerText
                            .Split("|", StringSplitOptions.None)
                            .Select(x => x.Trim().Substring(0, 1))
                            .ToList();

                        if (!letters.Any())
                        {
                            Logger.LogError($"Could not determine {nameof(letters)}.");
                            return new ZacksResponse(value.Ticker);
                        }
                        if (letters.Count != 4)
                        {
                            Logger.LogError($"Could not determine a exactly 4 {nameof(letters)} but {letters.Count}.");
                            return new ZacksResponse(value.Ticker);
                        }

                        valueString = letters.First();
                        growthString = letters.Skip(1).First();
                        momentumString = letters.Skip(2).First();
                        vgmString = letters.Skip(3).First();
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
            }

            return new ZacksResponse(
                value.Ticker,
                rank,
                valueString,
                growthString,
                momentumString,
                vgmString,

                averagePriceTargetString,
                highestPriceTargetString,
                lowestPriceTargetString,
                priceTargetPercentageString,

                strongBuyString,
                buyString,
                holdString,
                sellString,
                strongSellString,
                averageBrokerRecommendationString,
                true);
        }
    }
}