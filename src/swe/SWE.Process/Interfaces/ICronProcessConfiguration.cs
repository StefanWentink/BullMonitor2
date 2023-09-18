using SWE.Process.Models;

namespace SWE.Process.Interfaces
{
    public interface ICronProcessConfiguration
    {
        CronCommandConfiguration[] Cron { get; set; }
    }
}