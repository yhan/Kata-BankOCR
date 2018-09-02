using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOcr
{
    public class AccountComputation
    {
        private int _checksum;

        public AccountComputation(List<DigitComputation> digits)
        {
            Digits = digits;
        }

        public List<DigitComputation> Digits { get; }

        public List<int[]> Combinations { get; } = new List<int[]>();

        public string AsString()
        {
            var builder = new StringBuilder();

            var accountsCandidates = FormatAccount().ToArray();
            var length = accountsCandidates.Length;

            for (var i = 0; i < length; i++)
            {
                var foo = length == i + 1 ? string.Empty : "|";
                builder.Append($"{accountsCandidates[i]}{foo}");
            }

            return builder.ToString();
        }


        private IEnumerable<string> FormatAccount()
        {
            var computeCombinations = ComputeCombinations();

            foreach (var computeCombination in computeCombinations)
            {
                var one = string.Join(string.Empty, computeCombination.Select(x => { return x.ToString(); }));

                if (IsIll())
                {
                    one = one + " ILL";
                }

                if (!IsValid())
                {
                    one = one + " ERR";
                }

                yield return one;
            }
        }

        public bool IsValid()
        {
            _checksum = Digits.Sum(d => d.ChecksumWeight * d.Numerics.Single()) % 11;
            return _checksum == 0;
        }

        private bool IsIll()
        {
            return Digits.Any(x => x.IsIllegal);
        }

        public List<Account> ComputePossibleAccounts()
        {
            var accounts = new List<Account>();
            var computeCombinations = ComputeDigitCombinations();

            foreach (var cleanDigits in computeCombinations)
            {
                var cleanAccount = new Account(cleanDigits);
                accounts.Add(cleanAccount);
            }

            return accounts;
        }


        public List<List<int>> ComputeCombinations()
        {
            var combinations = new List<List<int>>();

            while (combinations.All(x => x.Count != 9))
            {
                var oneCombination = new List<int>();
                combinations.Add(oneCombination);
                foreach (var digit in Digits)
                {
                    var numeric = 0;
                    var queue = digit.AsQueue();


                    if (queue.Count == 1)
                    {
                        numeric = queue.Dequeue();

                        combinations.ForEach(x => x.Add(numeric));
                        continue;
                    }

                    while (queue.Count > 0)
                    {
                        numeric = queue.Dequeue();

                        if (AllHaveTheSameLength(combinations))
                        {
                            var newCombination = Clone(oneCombination);

                            combinations.Add(newCombination);

                            oneCombination.Add(numeric);
                        }
                        else
                        {
                            SmallestCombination(combinations).Add(numeric);
                        }
                    }
                }
            }

            return combinations;
        }

        public List<List<Digit>> ComputeDigitCombinations()
        {
            var combinations = new List<List<Digit>>();

            while (combinations.All(x => x.Count != 9))
            {
                var oneCombination = new List<Digit>();
                combinations.Add(oneCombination);

                var weight = 9;
                foreach (var digit in Digits)
                {
                    var numeric = 0;
                    var queue = digit.AsQueue();

                    if (queue.Count == 1)
                    {
                        numeric = queue.Dequeue();

                        combinations.ForEach(x => x.Add(new Digit(numeric, weight)));
                        continue;
                    }

                    while (queue.Count > 0)
                    {
                        numeric = queue.Dequeue();
                        var cleanDigit = new Digit(numeric, weight);

                        if (AllHaveTheSameLength(combinations))
                        {
                            var newCombination = Clone(oneCombination);

                            combinations.Add(newCombination);

                            oneCombination.Add(cleanDigit);
                        }
                        else
                        {
                            SmallestCombination(combinations).Add(cleanDigit);
                        }
                    }

                    weight--;
                }
            }

            return combinations;
        }

        private bool AllHaveTheSameLength<T>(List<List<T>> combinations)
        {
            var count = combinations.First().Count;
            return combinations.All(x => x.Count == count);
        }

        private static List<T> SmallestCombination<T>(List<List<T>> combinations)
        {
            var min = combinations.Min(x => x.Count);
            return combinations.Single(x => x.Count == min);
        }


        private static List<T> Clone<T>(List<T> oneCombination) where T : struct
        {
            var cloned = new List<T>();
            oneCombination.ForEach(x => cloned.Add(x));
            return cloned;
        }
    }

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
            var one = string.Join(string.Empty, _cleanDigits.Select(x => { return x.ToString(); }));

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

    public struct Digit
    {
        public int Numeric { get; }
        public int Weight { get; }
        public bool IsIllegal => false; //TODO 

        public Digit(int numeric, int weight)
        {
            Numeric = numeric;
            Weight = weight;
        }

        public override string ToString()
        {
            return Numeric.ToString();
        }
    }
}