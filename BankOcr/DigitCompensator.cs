using System.Collections.Generic;

namespace BankOcr
{
    public class DigitCompensator
    {
        private readonly Dictionary<int, string> _referential = new Dictionary<int, string>
        {
            [0] = " _ | ||_|",
            [1] = "     |  |",
            [2] = " _  _||_ ",
            [3] = " _  _| _|",
            [4] = "   |_|  |",
            [5] = " _ |_  _|",
            [6] = " _ |_ |_|",
            [7] = " _   |  |",
            [8] = " _ |_||_|",
            [9] = " _ |_| _|",
        };

        internal IEnumerable<int> Compensate(string toCompensate)
        {
            foreach (var numericAndString in _referential)
            {
                var numeric = numericAndString.Key;
                string referential = numericAndString.Value;
                if (CanMatch(toCompensate, referential))
                {
                    yield return numeric;
                }
            }
        }

        /// <summary>
        /// a digit in one line has 3*3=9 chars, if <paramref name="toCompensate"/> and <paramref name="referential"/> perfectly match, return true.
        /// if only one char does not match between the two, matching degree is 8.
        /// That's to say, with add or removing a bar or underscore, <paramref name="toCompensate"/> can be compensated to <paramref name="referential"/>
        /// </summary>
        /// <param name="toCompensate"></param>
        /// <param name="referential"></param>
        /// <returns></returns>
        private bool CanMatch(string toCompensate, string referential)
        {
            if (toCompensate == referential)
            {
                //itself: no need to compensate as we won't want to yield a legal digit itsef
                return false;
            }

            int matchingDegree = 0;

            for (int i = 0; i < referential.Length; i++)
            {
                if (toCompensate[i] == referential[i])
                {
                    matchingDegree++;
                }
            }

            return matchingDegree == 8;
        }
    }
}