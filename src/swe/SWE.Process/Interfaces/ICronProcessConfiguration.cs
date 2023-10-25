using SWE.Process.Configurations;

namespace SWE.Process.Interfaces
{
    public interface ICronProcessConfiguration
    {
        CronCommandConfiguration[] Cron { get; set; }
    }
}