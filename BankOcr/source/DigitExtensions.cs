using System.Collections.Generic;

namespace BankOcr.source
{
    public static class DigitExtensions
    {
        public static Digit AsDigit(this int numeric, int weight)
        {
            return new Digit(numeric, weight);
        }

        public static List<Digit> AsDigits(this List<int> numerics)
        {
            return new List<Digit>
            {
                numerics[0].AsDigit(9),
                numerics[1].AsDigit(8),
                numerics[2].AsDigit(7),
                numerics[3].AsDigit(6),
                numerics[4].AsDigit(5),
                numerics[5].AsDigit(4),
                numerics[6].AsDigit(3),
                numerics[7].AsDigit(2),
                numerics[8].AsDigit(1)
            };
        }
    }
}