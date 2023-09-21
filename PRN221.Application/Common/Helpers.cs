using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN221.Application.Common
{
    public class Helpers
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public static bool IsNumberPositive(string text)
        {
            return int.TryParse(text, out int number) && number > 0;
        }
        public static bool IsByte(string text)
        {
            return byte.TryParse(text, out byte _);
        }

        public static string patternYear = @"^(?:[1-9]\d{3}|0\d{3})$";

        public static bool IsYear(string input)
        {
            return Regex.IsMatch(input, patternYear);
        }

        public static bool IsDecimal(string input)
        {
            return decimal.TryParse(input, out decimal _);
        }
    }
}
