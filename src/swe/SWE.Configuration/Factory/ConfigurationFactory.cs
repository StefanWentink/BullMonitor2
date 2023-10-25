using Microsoft.Extensions.Configuration;
using MoreLinq.Extensions;
using System;

namespace SWE.Configuration.Factory
{
    public static class ConfigurationFactory
    {
        public static IConfigurationBuilder Create(
            string? environmentName = null,
            string? basePath = null,
            bool environmentOptional = false) // guarding there is an environment config by default, just in case
        {
            if (basePath == null)
            {
                basePath = Directory.GetCurrentDirectory();
            }
            else if (!Directory.Exists(basePath))
            {
                basePath = Directory.GetCurrentDirectory() + "/PackageRoot/Config/";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .SetFiles(environmentName, environmentOptional);


            return builder;
        }

        public static IConfigurationBuilder SetFiles(
            this IConfigurationBuilder configurationBuilder,
            string? environmentName = null,
            bool environmentOptional = false) // guarding there is an environment config by default, just in case
        {
            Environment.SetEnvironmentVariable("RunEnvironment", environmentName ?? "local");

            if (environmentName?.Contains("dev", StringComparison.OrdinalIgnoreCase) == true)
            {
                environmentOptional = true;
            }

            configurationBuilder
                .AddJsonFiles(environmentName, environmentOptional);

            //Environment.SetEnvironmentVariable("RunEnvironment", environmentName ?? "local");

#if DEBUG
            configurationBuilder
                .Sources
                .Where(source => source is FileConfigurationSource _)
                .Select(source => source as FileConfigurationSource)
                .Select(fileConfigurationSource => fileConfigurationSource?.Path)
                .Distinct()
                .OrderBy(x => x?.Length ?? 0)
                .ForEach(path => Console.Write($"Configuration loaded: {path}.{Environment.NewLine}"));
#endif

            return configurationBuilder;
        }

        public static IConfigurationBuilder AddJsonFiles(
            this IConfigurationBuilder builder,
            string? environment = null,
            bool environmentOptional = false, // guarding there is an environment config by default, just in case
            string? infix = null)
        {
            builder.AddJsonFile($"appsettings{infix}.json", optional: true);

            if (environment != null)
            {
                builder = builder
                    .AddJsonFile($"appsettings{infix}.{environment}.json", optional: environmentOptional);
            }

            builder.AddJsonFile($"appsettings.local.json", optional: true);

            return builder;
        }
    }
}