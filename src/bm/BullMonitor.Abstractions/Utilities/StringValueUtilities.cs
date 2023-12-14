using System.Globalization;

namespace BullMonitor.Abstractions.Utilities
{
    internal static class StringValueUtilities
    {
        private static readonly NumberFormatInfo _numberFormatInfo = new CultureInfo("en-US", false ).NumberFormat;
        private static NumberStyles currencyStyle = NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol;
        private static NumberStyles percentageStyle = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
        
        internal static decimal ToAmount(string value)
        {
            try
            {
                if (decimal.TryParse(value, currencyStyle, _numberFormatInfo, out var _amount))
                {
                    return _amount;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return 0;
        }

        internal static decimal ToPercentage(string value)
        {
            var percentageString = value.Replace("%", "").Trim();

            try
            {
                if (decimal.TryParse(percentageString, percentageStyle, _numberFormatInfo, out var _amount))
                {
                    return _amount;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return 0;
        }

        internal static int ToInteger(string value)
        {
            try
            {
                if (int.TryParse(value.Trim(), out var _integer))
                {
                    return _integer;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return 0;
        }
    }
}
