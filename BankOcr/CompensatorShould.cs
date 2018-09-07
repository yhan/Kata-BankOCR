using System.Collections.Generic;

namespace BankOcr
{
    using System.IO;

    using NFluent;

    using NUnit.Framework;

    class CompensatorShould
    {
        [Test]
        public void Return_1_when_input_is_ill_single_pipe()
        {
            var digit = new Digit(File.ReadAllLines(@"reference_asciiarts\1_ill_1.txt"));
            var compensator = new Compensator();
            IList<int> actual = compensator.Convert(digit);
            Check.That(actual).ContainsExactly(1);
        }

        [Test]
        public void Return_2_when_input_is_ill()
        {
            var digit = new Digit(File.ReadAllLines(@"reference_asciiarts\2_ill.txt"));
            var compensator = new Compensator();
            IList<int> actual = compensator.Convert(digit);
            Check.That(actual).ContainsExactly(2);
        }
    }
}