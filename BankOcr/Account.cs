namespace BankOcr
{
    using System.Collections.Generic;
    using System.Data.Odbc;
    using System.Linq;
    using System.Runtime.Remoting;

    public class Account
    {
        private int _checksum;

        public List<Digit> DigitsValues { get; }

        public Account(List<Digit> digitsValues)
        {
            this.DigitsValues = digitsValues;
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
                foreach (var digit in DigitsValues)
                {
                    if (digit.GetNumeric() == 9)
                    {
                        digit.ForceValue(8);
                        if (IsValid())
                        {
                            return FormatAccount();
                        }
                        digit.ForceValue(null);
                    }

                    if (digit.GetNumeric() == 8)
                    {
                        digit.ForceValue(0);
                        if (IsValid())
                        {
                            return FormatAccount();
                        }
                        digit.ForceValue(null);
                    }
                }
                return t + " ERR";
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