namespace BullMonitor.Ticker.Process.Models
{
    internal class FileTickerModel
    {
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
    }
}