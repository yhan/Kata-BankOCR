using System.Collections.Generic;
using System.Linq;
using BankOcr.source;
using NFluent;
using NUnit.Framework;

namespace BankOcr
{
    [TestFixture]
    public class AccountDigitsCombinationsShould
    {
        [Test]
        public void Account_488067775_when_8_can_be_considered_as_0()
        {
            var account = new AccountComputation(new List<DigitComputation> {new DigitComputation(new HashSet<int> {4}, 9), new DigitComputation(new HashSet<int> {8, 0}, 8), new DigitComputation(new HashSet<int> {8, 0}, 7), new DigitComputation(new HashSet<int> {0}, 6), new DigitComputation(new HashSet<int> {6}, 5), new DigitComputation(new HashSet<int> {7}, 4), new DigitComputation(new HashSet<int> {7}, 3), new DigitComputation(new HashSet<int> {7}, 2), new DigitComputation(new HashSet<int> {5}, 1)});


            var allCombinations = account.ComputeDigitCombinations();

            Check.That(allCombinations).HasSize(4);
            Check.That(allCombinations[0].Select(x => x.Numeric)).ContainsExactly(4, 8, 8, 0, 6, 7, 7, 7, 5);
            Check.That(allCombinations[1].Select(x => x.Numeric)).ContainsExactly(4, 8, 0, 0, 6, 7, 7, 7, 5);
            Check.That(allCombinations[2].Select(x => x.Numeric)).ContainsExactly(4, 0, 8, 0, 6, 7, 7, 7, 5);
            Check.That(allCombinations[3].Select(x => x.Numeric)).ContainsExactly(4, 0, 0, 0, 6, 7, 7, 7, 5);
        }

        [Test]
        public void Account_490067775_when_9_can_be_considered_as_8()
        {
            var account = new AccountComputation(new List<DigitComputation> {new DigitComputation(new HashSet<int> {4}, 9), new DigitComputation(new HashSet<int> {9, 8}, 8), new DigitComputation(new HashSet<int> {0}, 7), new DigitComputation(new HashSet<int> {0}, 6), new DigitComputation(new HashSet<int> {6}, 5), new DigitComputation(new HashSet<int> {7}, 4), new DigitComputation(new HashSet<int> {7}, 3), new DigitComputation(new HashSet<int> {7}, 2), new DigitComputation(new HashSet<int> {5}, 1)});

            var combinations = account.ComputeDigitCombinations();
            Check.That(combinations).HasSize(2);

            Check.That(combinations[0].Select(x => x.Numeric)).ContainsExactly(4, 9, 0, 0, 6, 7, 7, 7, 5);
            Check.That(combinations[1].Select(x => x.Numeric)).ContainsExactly(4, 8, 0, 0, 6, 7, 7, 7, 5);
        }
    }
}