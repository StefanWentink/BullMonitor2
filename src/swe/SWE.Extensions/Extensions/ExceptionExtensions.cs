namespace SWE.Extensions.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception GetInnerMostException(
            this Exception exception)
        {
            return exception
                .GetInnerExceptions()
                .LastOrDefault() ?? exception;
        }

        private static IEnumerable<Exception> GetExceptions(
            this Exception exception)
        {
            if (exception != default)
            {
                var innerException = exception.InnerException;

                foreach (var inner in innerException?
                    .GetInnerExceptions() ??  Enumerable.Empty<Exception>())
                {
                    yield return inner;
                }
            }
        }

        public static IEnumerable<Exception> GetInnerExceptions(
            this Exception exception)
        {
            if (exception != default)
            {
                yield return exception;

                if (exception is AggregateException aggregateException)
                {
                    foreach (var innerAggregate in aggregateException.GetAggregateInnerExceptions().SelectMany(x => x))
                    {
                        yield return innerAggregate;
                    }
                }
                else
                {
                    foreach (var innerException in exception.GetExceptions())
                    {
                        yield return innerException;
                    }
                }
            }
        }

        private static IEnumerable<IEnumerable<Exception>> GetAggregateInnerExceptions(
            this AggregateException exception)
        {
            if (exception != default)
            {
                foreach (var innerException in exception.InnerExceptions)
                {
                    yield return innerException.GetInnerExceptions();
                }
            }
        }
    }
}