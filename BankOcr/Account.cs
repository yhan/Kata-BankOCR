namespace BankOcr
{
    using System.Collections.Generic;
    using System.Data.Odbc;
    using System.Linq;
    using System.Runtime.Remoting;

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

        public string AsString()
        {

            var t = FormatAccount();

            if (IsIll())
            {
                return t + " ILL";
            }

            if (!IsValid())
            {
                var validAccounts = _accountCompensator.Compensate(this).ToArray();

                if(validAccounts.Length == 0)
                {
                    return t + " ERR";
                }

                if (validAccounts.Length == 1)
                {
                    return validAccounts[0].FormatAccount();
                }

                var validAccountsRepresentations = string.Join(", ", validAccounts.Select(x => x.FormatAccount()));
                return $"{validAccountsRepresentations} AMB";
            }

            return t;
        }

        private string FormatAccount()
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