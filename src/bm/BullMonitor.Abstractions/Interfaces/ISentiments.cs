namespace BullMonitor.Abstractions.Interfaces
{
    public interface ISentiments
    {
        int BullishSentiments { get; }
        int NeutralSentiments { get; }
        int BearishSentiments { get; }

        int SentimentsScore { get; }
        decimal AverageSentiments { get ; }
    }
}