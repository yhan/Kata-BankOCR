using System;
using System.Collections.Generic;

namespace BankOcr.AsciiReaders
{
    public class HeaderReader
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