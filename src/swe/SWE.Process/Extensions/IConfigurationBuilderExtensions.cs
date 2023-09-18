using Microsoft.Extensions.Configuration;
using SWE.Configuration.Factory;

namespace SWE.Process.Extensions
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAppsettingsFile(
            this IConfigurationBuilder configurationBuilder,
            string? environmentName)
        {
            configurationBuilder.AddJsonFiles(environmentOptional: false);
            configurationBuilder.AddJsonFiles(environmentName, true);

            return configurationBuilder;
        }
    }
}