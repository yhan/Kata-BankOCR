namespace BankOcr
{
    using System;
    using System.Collections.Generic;

    public class BodyReader : IOcrLineReader
    {
        public HashSet<int> Read(string input)
        {
            switch (input)
            {
                case "| |":
                    return new HashSet<int> { 0 };
                case "  |":
                    return new HashSet<int> { 1, 7 };
                case " _|":
                    return new HashSet<int> { 2, 3 };
                case "|_|":
                    return new HashSet<int>
                               {
                                   4, 8, 9,
                                   /*correction*/
                                   0
                               };
                case "|_ ":
                    return new HashSet<int> { 5, 6 };
            }

            throw new ArgumentException();
        }
    }
}