namespace BankOcr
{
    using System;
    using System.Collections.Generic;

    public class FooterReader : IOcrLineReader
    {
        public HashSet<int> Read(string input)
        {
            switch (input)
            {
                case "|_|":
                    return new HashSet<int> { 0, 6, 8 };
                case "  |":
                    return new HashSet<int> { 1, 4, 7 };
                case "|_ ":
                    return new HashSet<int> { 2 };
                case " _|":
                case " _ ":
                    return new HashSet<int> { 3, 5, 9 };
            }

            return new HashSet<int>();
        }
    }
}