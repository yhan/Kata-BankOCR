using System.IO;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace BankOcr.Tests
{
    // TODO: use test cases of http://codingdojo.org/kata/BankOCR/
    [TestFixture]
    public class AccountCompensationShould
    {
        [Test]
        public void Return_corrected_account_number_when_a_digit_is_missing_a_bar()
        {
            var bankOcrLine = new OcrReader();
            var accountAsString = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\123956189_ILL.txt")).Single();

            Check.That(accountAsString.ToString()).IsEqualTo("723956189, 123956188 AMB");
        }

        [Test]
        public void Returns_account_as_a_valid_account_when_a_valid_digit_results_an_invalid_account()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\488067775.txt")).Single();
            Check.That(actualAccount.ToString()).IsEqualTo("480067775, 488087775, 488067715 AMB");
        }

        [Test]
        public void Returns_account_as_a_valid_account_when_input_using_490067775_with_one_flur_digit()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines(@"reference_asciiarts\490067775.txt")).Single();
            Check.That(actualAccount.ToString()).IsEqualTo("480067775");
        }
    }
}