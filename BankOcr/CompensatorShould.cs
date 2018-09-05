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
            var compensator = new Compensator(File.ReadAllLines(@"reference_asciiarts\1_ill_1.txt"));
            int actual = compensator.Convert();
            Check.That(actual).IsEqualTo(1);
        }

        [Test]
        public void Return_2_when_input_is_ill()
        {
            var compensator = new Compensator(File.ReadAllLines(@"reference_asciiarts\2_ill.txt"));
            int actual = compensator.Convert();
            Check.That(actual).IsEqualTo(2);
        }
    }
}