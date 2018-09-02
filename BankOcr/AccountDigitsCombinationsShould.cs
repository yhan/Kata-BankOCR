using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

namespace BankOcr
{
    [TestFixture]
    public class AccountDigitsCombinationsShould
    {
        [Test]
        public void Account_490067775_when_9_can_be_considered_as_8()
        {
            var account = new AccountComputation(new List<DigitComputation>
            {
                new DigitComputation(new HashSet<int> {4}, 9),
                new DigitComputation(new HashSet<int> {9, 8}, 8),
                new DigitComputation(new HashSet<int> {0}, 7),
                new DigitComputation(new HashSet<int> {0}, 6),
                new DigitComputation(new HashSet<int> {6}, 5),
                new DigitComputation(new HashSet<int> {7}, 4),
                new DigitComputation(new HashSet<int> {7}, 3),
                new DigitComputation(new HashSet<int> {7}, 2),
                new DigitComputation(new HashSet<int> {5}, 1)
            });

            var combinations = account.ComputeCombinations();
            Check.That(combinations).HasSize(2);
            Check.That(combinations)
                .Contains(new List<int> {4, 9, 0, 0, 6, 7, 7, 7, 5}, new List<int> {4, 8, 0, 0, 6, 7, 7, 7, 5});
        }
    }
}