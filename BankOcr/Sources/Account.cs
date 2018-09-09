using System.Collections.Generic;
using System.Linq;

namespace BankOcr.Sources
{
    public class Account
    {
        private int _checksum;
        private readonly AccountCompensator _accountCompensator = new AccountCompensator();
        

        public List<Digit> DigitsValues { get; }

        public Account(List<Digit> digitsValues)
        {
            this.DigitsValues = digitsValues;
        }

        public Account(int[] numerics)
        {
            this.DigitsValues = numerics.Select((x, index) =>
            {
                int checksum = 9 - index;
                var digit = new Digit(x, checksum);

                return digit;
            }).ToList();
        }

        public override string ToString()
        {
            string accountRepresentation = FormatUserStory3Account();

            if(ContainsNoIllegalDigit() && this.IsValid())
            {
                return accountRepresentation;
            }

            if (IsIll())
            {
                var accounCombinationComputer = new AccounCombinationComputer(this);
                var accountsContainingOnlyLegalDigits = accounCombinationComputer.ComputeCombinations();

                var validAccounts = accountsContainingOnlyLegalDigits.SelectMany(acc => _accountCompensator.Compensate(acc)).ToArray();
                return Format(validAccounts, accountRepresentation);
            }

            if (!IsValid())
            {
                var validAccounts = _accountCompensator.Compensate(this).ToArray();

                return Format(validAccounts, accountRepresentation);
            }

            return accountRepresentation;
        }

        private bool ContainsNoIllegalDigit()
        {
            return this.DigitsValues.All(x => !x.IsIllegal);
        }

        private static string Format(IReadOnlyList<Account> validAccounts, string mayContainsIllegalDigitAccountRepresentation)
        {
            if (validAccounts.Count == 0)
            {
                return mayContainsIllegalDigitAccountRepresentation + " ILL";
            }

            if (validAccounts.Count == 1)
            {
                return validAccounts[0].FormatUserStory3Account();
            }

            var validAccountsRepresentations = string.Join(", ", validAccounts.Select(x => x.FormatUserStory3Account()));
            return $"{validAccountsRepresentations} AMB";
        }

        private string FormatUserStory3Account()
        {
            return string.Join(string.Empty, this.DigitsValues.Select(
                x =>
                    {
                        var numeric = x.GetNumeric();
                        if (!numeric.HasValue)
                        {
                            return "?";
                        }
                        return numeric.ToString();
                    }));
        }

        public bool IsValid()
        {
            this._checksum = DigitsValues.Sum(d => d.ChecksumWeight * d.GetNumeric().Value) % 11;
            var isValid = _checksum == 0;
            return isValid;
        }

        private bool IsIll()
        {
            return DigitsValues.Any(x => x.IsIllegal);
        }
    }
}