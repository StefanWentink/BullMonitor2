namespace BullMonitor.Abstractions.Interfaces
{
    public interface IAdvancedPurchaseAdvice
        : IBasicPurchaseAdvice
    {
        int StrongBuy { get; }
        int StrongSell { get; }
    }
}