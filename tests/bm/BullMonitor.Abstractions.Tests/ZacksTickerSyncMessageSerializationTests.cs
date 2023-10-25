using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Utilities;
using FluentAssertions;
using FluentAssertions.Equivalency;
using SWE.Tests.Factories;
using System.Text.Json;
using Xunit;

namespace BullMonitor.Abstractions.Tests
{
    public class ZacksTickerSyncMessageSerializationTests
    {
        [Fact]
        public void ZacksTickerSyncMessage_Should_Serialize()
        {
            var message = new ZacksTickerSyncMessage(
                GuidFactory.Create(1),
                "MSFT",
                new DateTimeOffset(2023, 10, 24, 0, 0, 0, TimeSpan.FromHours(2)),
                true);

            var serialized = JsonSerializer.Serialize(message);

            var actual = JsonSerializer.Deserialize<ZacksTickerSyncMessage>(serialized);

            actual.Should().BeEquivalentTo(message);
        }
    }
}