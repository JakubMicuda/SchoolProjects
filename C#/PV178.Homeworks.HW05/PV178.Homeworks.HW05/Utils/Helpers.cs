using System.Globalization;

namespace PV178.Homeworks.HW05.Utils
{
    public static class Helpers
    {
        public static double ParseDouble(string stringToParse)
        {
            double value;
            var x = double.TryParse(stringToParse, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            return value;
        }
    }
}
