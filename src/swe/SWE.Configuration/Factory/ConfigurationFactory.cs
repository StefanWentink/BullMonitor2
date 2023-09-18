using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE.Configuration.Factory
{
    public static class ConfigurationFactory
    {
        public static IConfigurationRoot Create(
            string? environment = null,
            string? basePath = null,
            bool environmentOptional = false // guarding there is an environment config by default, just in case
        )
        {
            if (basePath == null)
            {
                basePath = Directory.GetCurrentDirectory();
            }
            else if (!Directory.Exists(basePath))
            {
                basePath = Directory.GetCurrentDirectory() + "/PackageRoot/Config/";
            }

            if (environment?.Contains("dev", StringComparison.OrdinalIgnoreCase) == true)
            {
                environmentOptional = true;
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFiles(environment, environmentOptional)
                .AddJsonFiles("local", environmentOptional)
                .AddJsonFiles("mine", environmentOptional)
                .AddJsonFiles(environment, environmentOptional: true, infix: "-overrides");

            Environment.SetEnvironmentVariable("RunEnvironment", environment ?? "local");

#if DEBUG
            builder
                .Sources
                .Where(source => source is FileConfigurationSource _)
                .Select(source => source as FileConfigurationSource)
                .Select(fileConfigurationSource => fileConfigurationSource?.Path)
                .Distinct()
                .OrderBy(x => x?.Length ?? 0)
                .ForEach(path => Console.Write($"Configuration loaded: {path}.{Environment.NewLine}"));
#endif

            return builder.Build();
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