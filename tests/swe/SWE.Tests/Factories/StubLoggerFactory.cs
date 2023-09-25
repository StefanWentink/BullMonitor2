using Microsoft.Extensions.Logging;
using SWE.Tests.Stubs;

namespace SWE.Tests.Factories
{
    internal class StubLoggerFactory
    {
        internal static ILogger Create()
        {
            return new StubLogger();
        }

        internal static ILogger Create<T>()
        {
            return new StubLogger<T>();
        }
    }
}