using System;
using System.IO;
using System.Linq;
using BankOcr.source;
using NFluent;
using NUnit.Framework;

namespace BankOcr.tests
{
    [TestFixture]
    public class BankOcrShould
    {
        [Test]
        public void Returns_account_as_a_valid_account_when_input_using_488067775_with_one_defected_digit_8_should_be_corrected_0()
        {
            var bankOcrLine = new BankOCrReader();
            var compuatations = bankOcrLine.BuildAccountComputations(File.ReadAllLines($@"reference_asciiarts\488067775.txt"));

            Check.That(compuatations).HasSize(1);
            var accounts = compuatations[0].ComputePossibleAccounts();

            Check.That(accounts).HasSize(4);
            Check.That(accounts[0].AsPrintable()).IsEqualTo("488067775 ERR");
            Check.That(accounts[1].AsPrintable()).IsEqualTo("480067775");
            Check.That(accounts[2].AsPrintable()).IsEqualTo("408067775 ERR");
            Check.That(accounts[3].AsPrintable()).IsEqualTo("400067775 ERR");
        }

        [Test]
        public void Account_123956189_is_whatever_an_invalid_account_event_after_guessing_digit()
        {
            var bankOcrLine = new BankOCrReader();
            var actualAccount = bankOcrLine.BuildAccountComputations(File.ReadAllLines($@"reference_asciiarts\123956189.txt"));

            Check.That(actualAccount).HasSize(1);

            var accounts = actualAccount.Single().ComputePossibleAccounts();
            Check.That(accounts).HasSize(2);

            Check.That(accounts[0].AsPrintable()).IsEqualTo("123956189 ERR");
            Check.That(accounts[1].AsPrintable()).IsEqualTo("123956109 ERR");
        }

        [Test]
        public void Guess_digit_yields_an_invalid_account()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\list_of_accounts.txt");
            var ocrReader = new BankOCrReader();
            var accountComputations = ocrReader.BuildAccountComputations(readAllLines).ToArray();

            Check.That(accountComputations).HasSize(3);

            //123456789
            var accounts = accountComputations[0].ComputePossibleAccounts();
            Check.That(accounts).HasSize(2);
            Check.That(accounts[0].AsPrintable()).IsEqualTo("123456789");
            Check.That(accounts[1].AsPrintable()).IsEqualTo("123456709 ERR"); // Guess digit yields an invalid account

            //444444444
            accounts = accountComputations[1].ComputePossibleAccounts();
            Check.That(accounts).HasSize(1);
            Check.That(accounts[0].AsPrintable()).IsEqualTo("444444444 ERR");

            //000000000
            accounts = accountComputations[2].ComputePossibleAccounts();
            Check.That(accounts).HasSize(1);
            Check.That(accounts[0].AsPrintable()).IsEqualTo("000000000");
        }

        [Test]
        public void Guess_digit_yields_an_invalid_account_when_input_is_single_valid_account()
        {
            var bankOcrLine = new BankOCrReader();
            var accountComputations = bankOcrLine.BuildAccountComputations(File.ReadAllLines($@"reference_asciiarts\0123456789_correct_checksum_but_one_digit_flured.txt"));

            Check.That(accountComputations).HasSize(1);
            var computePossibleAccounts = accountComputations[0].ComputePossibleAccounts();
            Check.That(computePossibleAccounts).HasSize(2);

            Check.That(computePossibleAccounts[0].AsPrintable()).IsEqualTo("123456789");
            Check.That(computePossibleAccounts[1].AsPrintable()).IsEqualTo("123456709 ERR");
        }

        [Test]
        public void Return_account_number_123956189_when_file_contains_123956189()
        {
            var bankOcrLine = new BankOCrReader();
            var actual = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\444444444.txt"));
            Check.That(actual).ContainsExactly("444444444 ERR");
        }

        [Test]
        [Ignore("comeback")]
        public void Return_corrected_account_number_when_a_digit_is_missing_a_bar()
        {
            var bankOcrLine = new BankOCrReader();
            var accountAsString = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\123956189_ILL.txt")).Single();

            Check.That(accountAsString).IsEqualTo("123956188");
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        //[TestCase(8)]
        [TestCase(9)]
        public void Returns_digit_when_asciiart_digit_as_expected(int expectedDigit)
        {
            var digit = new DigitComputation(File.ReadAllLines($@"reference_asciiarts\{expectedDigit}.txt"));
            var actual = digit.Numerics.Single();
            Check.That(actual).IsEqualTo(expectedDigit);
        }

        [Test]
        public void Returns_2_digit_when_asciiart_digit_as_expected()
        {
            var expectedDigit = 8;
            var digit = new DigitComputation(File.ReadAllLines($@"reference_asciiarts\{expectedDigit}.txt"));
            var actualNumerics = digit.Numerics;

            Check.That(actualNumerics).HasSize(2);
            Check.That(actualNumerics).Contains(0, 8);
        }


        [Test]
        public void Return_invalid_account_as_a_string_tailed_by_ERR()
        {
            var bankOcrLine = new BankOCrReader();
            var actualAccount = bankOcrLine.BuildAccountComputations(File.ReadAllLines($@"reference_asciiarts\444444444.txt")).Single();

            Check.That(actualAccount.AsString()).IsEqualTo("444444444 ERR");
        }


        [Test]
        public void Return_true_when_Digit_is_illegile()
        {
            var digit = new DigitComputation(File.ReadAllLines(@"reference_asciiarts\1_ill.txt"));

            Check.That(digit.IsIllegal).IsTrue();
            Check.That(digit.GetNumericAsString()).IsEqualTo("?");
        }

        [Test]
        public void Returns_1_when_read_file_contains_asciiart_is_1()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\1.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(1);
        }

        [Test]
        public void Returns_2_when_read_file_contains_asciiart_is_2()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\2.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(2);
        }

        [Test]
        public void Returns_3_when_read_file_contains_asciiart_is_3()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\3.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(3);
        }

        [Test]
        public void Returns_4_when_read_file_contains_asciiart_is_4()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\4.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(4);
        }

        [Test]
        public void Returns_5_when_read_file_contains_asciiart_is_5()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\5.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(5);
        }

        [Test]
        public void Returns_6_when_read_file_contains_asciiart_is_6()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\6.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(6);
        }

        [Test]
        public void Returns_7_when_read_file_contains_asciiart_is_7()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\7.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(7);
        }

        [Test]
        public void Returns_8_when_read_file_contains_asciiart_is_8()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\8.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(8);
        }

        [Test]
        public void Returns_9_when_read_file_contains_asciiart_is_9()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\9.txt");
            var ocrReader = new BankOCrReader();
            var expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(9);
        }


        [Test]
        [Ignore("comeback")]
        public void Returns_account_as_a_valid_account_when_input_using_490067775_with_one_defected_digit_causing_ERROR_9_should_be_understood_as_8()
        {
            var bankOcrLine = new BankOCrReader();
            var actualAccount = bankOcrLine.BuildAccountComputations(File.ReadAllLines($@"reference_asciiarts\490067775.txt")).Single();
            Check.That(actualAccount.AsString()).IsEqualTo("480067775");
        }


        [Test]
        public void Throws_when_not_all_lines_contains_exactly_27_characters()
        {
            var bankOcrLine = new BankOCrReader();

            Check.ThatCode(() =>
            {
                var actual = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\invalidAccount_123956189_allLineShouldHaveExactly27Characters.txt")).ToArray();
            }).Throws<ArgumentException>().WithMessage("All lines should have exactly 27 characters. they are:   | _| _||_||_ | _   ||_||_|");
        }

        [Test]
        public void Throws_when_not_the_4th_line_of_a_digit_source_is_not_blank()
        {
            var bankOcrLine = new BankOCrReader();

            Check.ThatCode(() => { bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\invalidAccount_123956189_forAllDigitSource_the_4th_line_should_be_blank.txt")).ToArray(); }).Throws<ArgumentException>().WithMessage("Each account is 4 lines long");
        }


        [TestCase("480067775")]
        //[Ignore("")]
        public void CheckHelper(string account)
        {
            var multiplicatonOperand = 9;
            var checksum = 0;

            foreach (var t in account.ToCharArray())
            {
                var multiplyBy = multiplicatonOperand--;
                checksum += (int) (char.GetNumericValue(t) * multiplyBy);
            }

            Check.That(checksum % 11).IsEqualTo(0);
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var dir = Path.GetDirectoryName(typeof(BankOcrShould).Assembly.Location);
            TestContext.WriteLine($"Current dir = '{dir}'");
            Environment.CurrentDirectory = dir;
        }
    }
}