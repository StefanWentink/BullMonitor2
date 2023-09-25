namespace SWE.Mongo.Extensions
{
    public static class MongoTimeSerieFieldExtensions
    {
        public static DateTime ToDateTimeKindUtc(
            this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToUniversalTime().UtcDateTime;
        }
    }
}