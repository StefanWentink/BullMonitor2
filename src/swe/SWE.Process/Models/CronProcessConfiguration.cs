using SWE.Process.Interfaces;

namespace SWE.Process.Models
{
    public class CronProcessConfiguration
        : ICronProcessConfiguration
    {
        public CronCommandConfiguration[] Cron { get; set; } = Array.Empty<CronCommandConfiguration>();
    }
}