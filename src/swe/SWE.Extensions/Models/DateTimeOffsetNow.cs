using SWE.Extensions.Interfaces;

namespace SWE.Extensions.Models
{
    public class DateTimeOffsetNow
        : IDateTimeOffsetNow
    {
        private DateTimeOffset? _now = null;

        public DateTimeOffsetNow()
        { }

        /// <summary>
        /// Overrideing with specific DateTimeOffset
        /// </summary>
        public DateTimeOffsetNow(DateTimeOffset now)
        {
            _now = now;
        }

        public DateTimeOffset Now => _now ?? DateTimeOffset.Now;
    }
}