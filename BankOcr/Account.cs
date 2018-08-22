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

            this._checksum = DigitsValues.Sum(d => d.ChecksumWeight * d.GetNumeric());
        }

        public string AsString()
        {
            return string.Join(string.Empty, this.DigitsValues.Select(x => x.GetNumeric().ToString()));
        }

        public bool IsValid()
        {
            var isValid = _checksum % 11 == 0;
            return isValid ;
        }
    }
}