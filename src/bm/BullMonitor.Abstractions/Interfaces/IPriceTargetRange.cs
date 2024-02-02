namespace BullMonitor.Abstractions.Interfaces
{
    public interface IPriceTargetRange
        : IPriceTarget
    {
        decimal LowestPriceTarget { get; }
        decimal HighestPriceTarget { get; }
        decimal PercentagePriceTarget { get; }
    }
}