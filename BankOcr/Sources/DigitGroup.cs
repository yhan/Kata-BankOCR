using System.Collections.Generic;
using System.Linq;

namespace BankOcr.Sources
{
    public class DigitGroup
    {
        public DigitGroup(Digit digit)
        {
            this.ChecksumWeight = digit.ChecksumWeight;

            if (digit.IsIllegal)
            {
                this.Digits = digit.GetCompensatedNumerics().Select(x => new Digit(x, digit.ChecksumWeight));
            }
            else
            {
                this.Digits = new[] { digit };
            }

            this.Numerics = this.Digits.Select(x => x.GetNumeric().Value).ToList();
        }

        public IEnumerable<Digit> Digits { get; private set; }

        public int ChecksumWeight { get; private set; }
        public IList<int> Numerics { get; set; }
    }
}