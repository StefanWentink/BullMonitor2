namespace BullMonitor.Abstractions.Interfaces
{
    public interface IRank
    {
        int Rank { get; }
        int Value { get; }
        int Growth { get; }
        int Momentum { get; }
        int VGM { get; }
    }
}