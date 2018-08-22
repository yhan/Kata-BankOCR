﻿namespace BankOcr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OcrReader
    {
        public int Read(string[] lines)
        {
            var line0 = lines[0];
            var line1 = lines[1];
            var line2 = lines[2];
            if (string.IsNullOrWhiteSpace(line0))
            {
                if ("  |" == line1)
                {
                    if ("  |" == line2)
                    {
                        return 1;
                    }
                }
            }

            if (" _ " == line0)
            {
                if (" _|" == line1)
                {
                    if ("|_ " == line2)
                    {
                        return 2;
                    }
                }
            }

            if (" _ " == line0)
            {
                if (" _|" == line1)
                {
                    if (" _|" == line2)
                    {
                        return 3;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(line0))
            {
                if ("|_|" == line1)
                {
                    if ("  |" == line2)
                    {
                        return 4;
                    }
                }
            }

            if (" _ " == line0)
            {
                if ("|_ " == line1)
                {
                    if (" _|" == line2)
                    {
                        return 5;
                    }
                }
            }

            if (" _ " == line0)
            {
                if ("|_ " == line1)
                {
                    if ("|_|" == line2)
                    {
                        return 6;
                    }
                }
            }

            if (" _ " == line0)
            {
                if ("  |" == line1)
                {
                    if ("  |" == line2)
                    {
                        return 7;
                    }
                }
            }

            if (" _ " == line0)
            {
                if ("|_|" == line1)
                {
                    if ("|_|" == line2)
                    {
                        return 8;
                    }
                }
            }

            if (" _ " == line0)
            {
                if ("|_|" == line1)
                {
                    if (" _|" == line2)
                    {
                        return 9;
                    }
                }
            }

            return 0;
        }

        public IEnumerable<string> ReadAccountsAsStrings(string[] lines)
        {
            var readAccountsAsStrings = ReadAccounts(lines).Select(account => account.AsString()).ToArray();
            return readAccountsAsStrings;

            //ThrowsWhenAnyNonEmptyLineDoesNotHave27Characters(lines);

            //var accounts = Parse(lines);
            //foreach (var account in accounts)
            //{
            //    yield return account.AsString();
            //}
        }

        public IEnumerable<Account> ReadAccounts(string[] lines)
        {
            ThrowsWhenAnyNonEmptyLineDoesNotHave27Characters(lines);

            return Parse(lines);
        }

        private static void ThrowsWhenAnyNonEmptyLineDoesNotHave27Characters(string[] lines)
        {
            IEnumerable<string> nonEmptyLines = lines.Where((l, index) => !IsThe4thLineOfEntry(index));

            if (nonEmptyLines.Any(x => x.Length != 27))
            {
                throw new ArgumentException("All lines should have exactly 27 characters.");
            }
        }

        private List<Account> Parse(string[] input)
        {
            var list = new List<Account>();
            for (int lineIndex = 0; lineIndex < input.Length; lineIndex += 4)
            {
                var digits = new List<Digit>(9);
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {
                    var header = input[lineIndex].Substring(columnIndex * 3, 3);
                    var body = input[lineIndex + 1].Substring(columnIndex * 3, 3);
                    var footer = input[lineIndex + 2].Substring(columnIndex * 3, 3);
                    string the4ThLine;
                    try
                    {
                        the4ThLine = input[lineIndex + 3];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new ArgumentException("Each account is 4 lines long");
                    }

                    ThrowsWhenThe4thLineIsNotBlank(the4ThLine);

                    digits.Add(new Digit(new[] {header, body, footer },  9 - columnIndex));
                }

                list.Add(new Account(digits));
            }

            return list;
        }

        private static bool IsThe4thLineOfEntry(int lineIndex)
        {
            return (lineIndex + 1) % 4 == 0;
        }

        private void ThrowsWhenThe4thLineIsNotBlank(string the4ThLine)
        {
            if (!string.IsNullOrWhiteSpace(the4ThLine))
            {
                throw new ArgumentException("The fourth line of each entry should be blank");
            }
        }
    }
}