using System.ComponentModel;
using System.Reflection;

namespace SWE.Extensions.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Will retrieve the <see cref="DescriptionAttribute"/> from an <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string? GetDescription<T>(
            this T? value,
            string? defaultValue = default)
            where T : Enum
        {
            if (value == null)
            {
                return defaultValue;
            }

            var enumMember = value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault();

            DescriptionAttribute? descriptionAttribute = enumMember?
                .GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descriptionAttribute?.Description ?? (defaultValue ?? value.ToString());
        }

        public static bool TryGetEnumValue<T>(
            this int value,
            out T? _enum)
            where T : struct, Enum
        {
            _enum = null;

            try
            {
                _enum = Enum
                    .GetValues<T>()
                    .Single(x => ((int)(object)x).Equals(value));

                return _enum != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryGetEnumValue<T>(
            string value,
            out T? _enum)
            where T : struct, Enum
        {
            _enum = null;

            if (int.TryParse(value, out var _intValue))
            {
                return TryGetEnumValue<T>(_intValue, out _enum);
            }

            if (Enum.TryParse<T>(value, out var _out))
            {
                _enum = _out;
                return true;
            }

            var descriptionDictionary = ToDescriptionDictionary<T>();
            var matchingDescription = descriptionDictionary
                .Where(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchingDescription.Count == 1)
            {
                _enum = matchingDescription.Single().Key;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try cast an integer as requested <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Will throw when not able to cast, and no <paramref name="defaultValue"/> provided.</exception>
        // TODO JK: move to IntExtensions and rename to ToEnum
        public static T GetEnumValue<T>(
            this int value,
            T? defaultValue = null)
            where T : struct, Enum
        {
            if (TryGetEnumValue<T>(value, out var _enum))
            {
                return (T)_enum;
            }

            if (defaultValue != null)
            {
                return defaultValue.Value;
            }

            throw new ArgumentException(
                $"{nameof(EnumExtensions)}.{nameof(GetEnumValue)}: {value} is not a valid value for {typeof(T).Name}.",
                nameof(value));
        }

        public static Dictionary<T, string?> ToDescriptionDictionary<T>()
            where T : struct, Enum
        {
            return Enum.GetValues<T>()
                .ToDictionary(x => x, x => x.GetDescription());
        }
    }
}