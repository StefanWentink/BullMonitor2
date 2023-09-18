namespace BullMonitor.Abstractions.Utilities
{
    public static class TickerUtilities
    {
        public static string CleanTicker(string ticker)
        {
            return ticker.Replace("^", "");
        }
    }
}