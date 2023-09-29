using System.Text.Json;

namespace SWE.Infrastructure.Abstractions.Factories
{
    public static class JsonSerializerOptionsFactory
    {
        private static JsonSerializerOptions? _options;
        public static JsonSerializerOptions Options => _options ??= CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
    }
}
