using Microsoft.Extensions.Configuration;

namespace SWE.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetConfigurationValue(
            this IConfiguration configuration,
            string rootTag,
            string valueTag)
        {
            return configuration
                .GetConfigurationValue(
                    new HashSet<string>
                    {
                        rootTag,
                        valueTag
                    });
        }

        public static string GetConfigurationValue(
            this IConfiguration configuration,
            IEnumerable<string> tags)
        {
            var configurationKey = tags.Where(x => !string.IsNullOrWhiteSpace(x)).Aggregate((x, y) => x + ':' + y);

            return configuration.TryGetConfigurationValue(configurationKey, out var _result)
                ? _result
                : string.Empty;
        }

        public static bool TryGetConfigurationValue(
            this IConfiguration configuration,
            string configKey,
            out string _value)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} should not be null.");
            }

            _value = configuration?
                .GetSection(configKey)
                .Value ?? string.Empty;

            return !string.IsNullOrWhiteSpace(_value);
        }

        public static bool GetBoolValue(
            this IConfiguration configuration,
            string rootTag,
            string valueTag,
            bool defaultValue)
        {
            return configuration
                .GetBoolValue(
                    rootTag,
                    string.Empty,
                    valueTag) ?? defaultValue;
        }

        public static bool? GetBoolValue(
            this IConfiguration configuration,
            string rootTag,
            string typeName,
            string valueTag)
        {
            var value = configuration.GetConfigurationValue(
                new HashSet<string>
                {
                    rootTag,
                    typeName,
                    valueTag
                });

            if (!string.IsNullOrWhiteSpace(value)
                && bool.TryParse(value, out var _response))
            {
                return _response;
            }

            return null;
        }

        public static int GetIntValue(
            this IConfiguration configuration,
            string rootTag,
            string valueTag,
            int defaultValue)
        {
            return configuration
                .GetIntValue(
                    rootTag,
                    string.Empty,
                    valueTag) ?? defaultValue;
        }

        public static int? GetIntValue(
            this IConfiguration configuration,
            string rootTag,
            string typeName,
            string valueTag)
        {
            var value = configuration.GetConfigurationValue(
                new HashSet<string>
                {
                    rootTag,
                    typeName,
                    valueTag
                });

            if (!string.IsNullOrWhiteSpace(value)
                && int.TryParse(value, out var _response))
            {
                return _response;
            }

            return null;
        }

        public static int? GetNullableIntValue(
            this IConfiguration configuration,
            string rootTag,
            string typeName,
            string valueTag)
        {
            return configuration.GetIntValue(
                 rootTag,
                 typeName,
                 valueTag) ??
                 configuration.GetIntValue(
                     rootTag,
                     string.Empty,
                     valueTag);
        }
    }
}
