namespace BankOcr
{
    using System;
    using System.IO;
    using System.Linq;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class BankOcrShould
    {
        [TestCase("480067775")]
        //[Ignore("")]
        public void CheckHelper(string account)
        {
            int multiplicatonOperand = 9;
            int checksum = 0;

            foreach (var t in account.ToCharArray())
            {
                var multiplyBy = multiplicatonOperand--;
                checksum += (int)(char.GetNumericValue(t) * multiplyBy);
            }

            Check.That(checksum % 11).IsEqualTo(0);
        }



        [Test]
        public void Return_true_when_Digit_is_illegile()
        {
            var digit = new Digit(File.ReadAllLines(@"reference_asciiarts\1_ill.txt"));

            Check.That(digit.IsIllegal).IsTrue();
            Check.That(digit.GetNumericAsString()).IsEqualTo("?");
        }


        [Test]
        //[Ignore("comeback")]
        public void Returns_account_as_a_valid_account_when_input_using_490067775_with_one_defected_digit_causing_ERROR()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\490067775.txt")).Single();
            Check.That(actualAccount.AsString()).IsEqualTo("480067775");
        }

        [Test]
        [Ignore("comeback")]
        public void Returns_account_as_a_valid_account_when_input_using_488067775_with_one_defected_digit()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\488067775.txt")).Single();
            Check.That(actualAccount.AsString()).IsEqualTo("480067775");
        }

        [Test]
        public void Returns_account_as_1_questionMark_3956189_when_input_using_123956189_ill()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\0123456789_correct_checksum_but_one_digit_flured.txt")).Single();
            Check.That(actualAccount.AsString()).IsEqualTo("123456789");
        }

        [Test]
        public void Return_invalid_account_as_a_string_tailed_by_ERR()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\444444444.txt")).Single();

            Check.That(actualAccount.AsString()).IsEqualTo("444444444 ERR");
        }

        [Test]
        public void Account_123956189_is_an_invalid_account()
        {
            var bankOcrLine = new OcrReader();
            var actualAccount = bankOcrLine.ReadAccounts(File.ReadAllLines($@"reference_asciiarts\123956189.txt")).Single();

            Check.That(actualAccount.IsValid()).IsFalse();
        }

        [Test]
        public void Return_account_number_123956189_when_file_contains_123956189()
        {
            var bankOcrLine = new OcrReader();
            var actual = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\444444444.txt"));
            Check.That(actual).ContainsExactly("444444444 ERR");
        }

        [Test]
        [Ignore("comeback")]
        public void Return_corrected_account_number_when_a_digit_is_missing_a_bar()
        {
            var bankOcrLine = new OcrReader();
            var accountAsString = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\123956189_ILL.txt")).Single();

            Check.That(accountAsString).IsEqualTo("123956188");
        }


        [Test]
        public void Throws_when_not_all_lines_contains_exactly_27_characters()
        {
            var bankOcrLine = new OcrReader();

            Check.ThatCode(
                    () =>
                        {
                            var actual = bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\invalidAccount_123956189_allLineShouldHaveExactly27Characters.txt")).ToArray();
                        })
                .Throws<ArgumentException>()
                .WithMessage("All lines should have exactly 27 characters. they are:   | _| _||_||_ | _   ||_||_|");
        }

        [Test]
        public void Throws_when_not_the_4th_line_of_a_digit_source_is_not_blank()
        {
            var bankOcrLine = new OcrReader();

            Check.ThatCode(
                    () =>
                        {
                             bankOcrLine.ReadAccountsAsStrings(File.ReadAllLines($@"reference_asciiarts\invalidAccount_123956189_forAllDigitSource_the_4th_line_should_be_blank.txt")).ToArray();
                        })
                .Throws<ArgumentException>()
                .WithMessage("Each account is 4 lines long");
        }

        [Test]
        public void Returns_1_when_read_file_contains_asciiart_is_1()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\1.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(1);
        }

        [Test]
        public void Returns_2_when_read_file_contains_asciiart_is_2()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\2.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(2);
        }

        [Test]
        public void Returns_3_when_read_file_contains_asciiart_is_3()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\3.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(3);
        }

        [Test]
        public void Returns_4_when_read_file_contains_asciiart_is_4()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\4.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(4);
        }

        [Test]
        public void Returns_5_when_read_file_contains_asciiart_is_5()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\5.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(5);
        }

        [Test]
        public void Returns_6_when_read_file_contains_asciiart_is_6()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\6.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(6);
        }

        [Test]
        public void Returns_7_when_read_file_contains_asciiart_is_7()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\7.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(7);
        }

        [Test]
        public void Returns_8_when_read_file_contains_asciiart_is_8()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\8.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(8);
        }

        [Test]
        public void Returns_9_when_read_file_contains_asciiart_is_9()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\9.txt");
            var ocrReader = new OcrReader();
            int expected = ocrReader.Read(readAllLines);
            Check.That(expected).IsEqualTo(9);
        }

        [Test]
        public void Returns_3_accounts_when_read_file_contains_12_lines_of_asciiarts()
        {
            var readAllLines = File.ReadAllLines(@"reference_asciiarts\list_of_accounts.txt");
            var ocrReader = new OcrReader();
            var expected = ocrReader.ReadAccountsAsStrings(readAllLines);
            Check.That(expected).Contains("123456789", "444444444 ERR", "000000000");
        }


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var dir = Path.GetDirectoryName(typeof(BankOcrShould).Assembly.Location);
            TestContext.WriteLine($"Current dir = '{dir}'");
            Environment.CurrentDirectory = dir;
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void Returns_digit_when_asciiart_digit_as_expected(int expectedDigit)
        {
            var digit = new Digit(File.ReadAllLines($@"reference_asciiarts\{expectedDigit}.txt"));
            int actual = digit.Numerics.Single();
            Check.That(actual).IsEqualTo(expectedDigit);
        }
    }
}