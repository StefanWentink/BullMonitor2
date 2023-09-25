using BullMonitor.Ticker.Process.Utilities;
using FluentAssertions;
using Xunit;

namespace BullMonitor.Ticker.Process.Tests
{
    public class CsvUtilitiesTests
    {
        private static string _text = "ACOR;Acorda Therapeutics Inc. Common Stock;United States;Health Care;Biotechnology: Biological Products (No Diagnostic Substances)\r\nACRS;Aclaris Therapeutics Inc. Common Stock;United States;Health Care;Major Pharmaceuticals\r\nACRX;AcelRx Pharmaceuticals Inc. Common Stock;United States;Health Care;Major Pharmaceuticals\r\nACST;Acasti Pharma Inc. Class A Common Stock;Canada;Health Care;Major Pharmaceuticals";

        [Fact]
        public void TextToTicker_Should_Resolve()
        {
            var actual = CsvUtilities
                .TextToTicker(_text, "NASDAQ");

            actual.Should().HaveCount(4);

            var item = actual.First();

            item.Exchange.Should().Be("NASDAQ");
            item.Ticker.Should().Be("ACOR");
            item.Name.Should().Be("Acorda Therapeutics Inc. Common Stock");
            item.Country.Should().Be("United States");
            item.Sector.Should().Be("Health Care");
            item.Industry.Should().Be("Biotechnology: Biological Products (No Diagnostic Substances)");
        }
    }
}