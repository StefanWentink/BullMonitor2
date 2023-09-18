using SWE.Extensions.Extensions;

namespace SWE.Extensions.Utilities
{
    public static class EnumUtilities
    {
        public static T GetEnumValue<T>(
            string? value)
            where T : struct, Enum
        {
            if (int.TryParse(value, out var _intValue))
            {
                if (EnumExtensions.TryGetEnumValue<T>(_intValue, out var _intResponse)
                    && _intResponse.HasValue)
                {
                    return _intResponse.Value;
                }
            }

            if (Enum.TryParse<T>(value, true, out var _stringResponse))
            {
                return _stringResponse;
            }

            var descriptionDictionary = EnumExtensions.ToDescriptionDictionary<T>();
            var matchingDescription = descriptionDictionary
                .Where(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchingDescription.Count == 1)
            {
                return matchingDescription.Single().Key;
            }


            throw new ArgumentException(
                $"{nameof(EnumExtensions)}.{nameof(GetEnumValue)}: {value} is not a valid value for {typeof(T).Name}.",
                nameof(value));
        }
    }
}