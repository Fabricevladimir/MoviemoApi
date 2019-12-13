using System.Globalization;

namespace Moviemo.API.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter (this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            char[] inputArray = input.ToCharArray();
            inputArray[0] = char.ToUpper(inputArray[0], CultureInfo.InvariantCulture);
            return new string(inputArray);
        }
    }
}
