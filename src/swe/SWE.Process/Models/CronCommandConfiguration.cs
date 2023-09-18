namespace SWE.Process.Models
{
    public class CronCommandConfiguration
    {
        public bool IsEnabled { get; set; } = true;
        public string Command { get; set; }
        /// <summary>
        /// Cronexpression: example; "*/30 * * * *" for every 30m on the hour 
        /// </summary>
        public string CronExpression { get; set; }

        [Obsolete("Only for serialisation.")]
        public CronCommandConfiguration()
            : this(
                 string.Empty,
                 string.Empty)
        { }

        public CronCommandConfiguration(
            string command,
            string cronExpression,
            bool isEnabled = true)
        {
            Command = command;
            CronExpression = cronExpression;
            IsEnabled = isEnabled;
        }
    }
}