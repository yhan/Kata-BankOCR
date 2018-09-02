using System.Collections.Generic;
using System.Linq;

namespace BankOcr
{
    public class Account
    {
        private readonly List<Digit> _cleanDigits;

        public Account(List<Digit> cleanDigits)
        {
            _cleanDigits = cleanDigits;
        }


        public bool IsValid()
        {
            var checksum = _cleanDigits.Sum(d => d.Weight * d.Numeric) % 11;
            return checksum == 0;
        }

        private bool IsIll()
        {
            return _cleanDigits.Any(x => x.IsIllegal);
        }

        public string AsPrintable()
        {
            var one = string.Join(string.Empty, _cleanDigits.Select(x => { return x.AsString(); }));

            if (IsIll())
            {
                one = one + " ILL";
            }

            if (!IsValid())
            {
                one = one + " ERR";
            }

            return one;
        }
    }
}