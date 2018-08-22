namespace BankOcr
{
    using System;
    using System.Collections.Generic;

    public class HeaderReader : IOcrLineReader
    {
        public HashSet<int> Read(string input)
        {
            switch (input)
            {
                case "   ":
                    return new HashSet<int> { 1, 4 };
                case " _ ":
                    return new HashSet<int> { 2, 3, 5, 6, 7, 8, 9, 0 };
            }

            throw new ArgumentException();
        }
    }
}