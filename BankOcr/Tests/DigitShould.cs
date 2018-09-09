using System.IO;
using BankOcr.Sources;
using NFluent;
using NUnit.Framework;

namespace BankOcr.Tests
{
    [TestFixture]
    public class DigitShould
    {
        //[TestCase(0)]
        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(3)]
        //[TestCase(4)]
        //[TestCase(5)]
        //[TestCase(6)]
        //[TestCase(7)]
        //[TestCase(8)]
        //[TestCase(9)]
        [Test]
        public void Return_digit_as_one_line()
        {
            var expectedDigit = 1;
            var digit = new Digit(File.ReadAllLines($@"reference_asciiarts\{expectedDigit}.txt"));
            var actual = digit.GetDigitAsStringInOneLine();
            Check.That(actual).IsEqualTo("     |  |");
        }

        [Test]
        public void Correct_illed_1_to_a_valid_1()
        {
            var digit = new Digit(File.ReadAllLines($@"reference_asciiarts\1_ILL.txt"));
            Check.That(digit.IsIllegal).IsTrue();
            Check.That(digit.GetNumeric()).IsNull();

            Check.That(digit.GetCompensatedNumerics()).ContainsExactly(1);
        }

        [Test]
        public void Nine_can_be_coompensated_to_8_5_or_3()
        {
            var digit = new Digit(File.ReadAllLines($@"reference_asciiarts\9.txt"));
            Check.That(digit.IsIllegal).IsFalse();
            Check.That(digit.GetNumeric()).IsEqualTo(9);

            Check.That(digit.GetCompensatedNumerics()).Contains(8, 5, 3);
        }
    }
}