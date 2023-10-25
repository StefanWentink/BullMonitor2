using SWE.Process.Interfaces;

namespace SWE.Process.Configurations
{
    public class CronProcessConfiguration
        : ICronProcessConfiguration
    {
        public CronCommandConfiguration[] Cron { get; set; } = Array.Empty<CronCommandConfiguration>();
    }
}