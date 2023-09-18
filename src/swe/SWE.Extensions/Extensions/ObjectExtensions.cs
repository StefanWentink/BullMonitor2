namespace SWE.Extensions.Extensions
{
    public static class ObjectExtensions
    {
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="self"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        public static IEnumerable<T> Yield<T>(this T self)
        {
            yield return self;
        }
    }
}