namespace BullMonitor.Data.Storage.Interfaces
{
    public interface ITickerValue
    {
        Guid TickerId { get; }
        DateTimeOffset Date { get; }
    }
}