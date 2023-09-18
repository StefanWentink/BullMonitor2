namespace BullMonitor.Data.Storage.Interfaces
{
    public interface IIdCode
    {
        Guid Id { get; }
        string Code { get; }
    }
}