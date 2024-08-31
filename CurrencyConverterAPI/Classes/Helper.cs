using System.Globalization;

namespace CurrencyConverterAPI.Classes
{
    public static class Helper
    {
        // Helper method to validate currency code format (ISO 4217)
        public static bool IsValidCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return false;

            return currencyCode.Length == 3 && currencyCode.All(char.IsLetter);
        }

        // Helper method to validate date format
        public static bool IsValidDateFormat(string date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}