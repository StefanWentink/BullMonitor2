using Microsoft.Extensions.Logging;

namespace SWE.Tests.Stubs
{
    public class StubLogger
        : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return state as IDisposable;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel > LogLevel.None;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"{logLevel}, {exception?.Message ?? formatter.Invoke(state, exception)}.");
        }
    }

    public class StubLogger<T>
        : StubLogger
        , ILogger<T>
    { }
}
