using Microsoft.Extensions.Configuration;
using SWE.Configuration.Extensions;

namespace SWE.Mongo.Extensions
{
    internal static class MongoConfigurationExtensions
    {
        internal static string GetMongoConnectionString(
            this IConfiguration configuration)
        {
            return configuration
                .GetConfigurationValue("ConnectionStrings", "Mongo");
        }
    }
}