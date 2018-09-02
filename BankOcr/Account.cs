namespace BankOcr
{
    using System.Collections.Generic;
    using System.Linq;

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
                return t + " ERR";
            }

            return t;
        }

        private string FormatAccount()
        {
            return string.Join(string.Empty, this.DigitsValues.Select(
                x =>
                    {
                        return x.GetNumericAsString();
                    }));
        }

        public bool IsValid()
        {
            this._checksum = DigitsValues.Sum(d => d.ChecksumWeight * d.Numeric.Value) % 11;
            return _checksum == 0;
        }

        private bool IsIll()
        {
            return DigitsValues.Any(x => x.IsIllegal);
        }
    }
}