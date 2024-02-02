namespace BullMonitor.Abstractions.Interfaces
{
    public interface IBasicPurchaseAdvice
    {
        int Buy { get; }
        int Hold { get; }
        int Sell { get; }
    }
}