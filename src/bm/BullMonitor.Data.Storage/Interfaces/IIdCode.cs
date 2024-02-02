namespace BullMonitor.Data.Storage.Interfaces
{
    public interface IIdCode
        : IId
    {
        string Code { get; }
    }
}