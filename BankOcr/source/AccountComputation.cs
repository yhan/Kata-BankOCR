using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOcr.source
{
    public class AccountComputation
    {
        private int _checksum;

        public AccountComputation(List<DigitComputation> digitComputations)
        {
            DigitComputations = digitComputations;
        }

        public List<DigitComputation> DigitComputations { get; }

        public List<List<Digit>> ComputeDigitCombinations()
        {
            var trees = BuildTrees(DigitComputations);
            var allCombinations = new List<List<Digit>>();
            foreach (var tree in trees)
            {
                var node = tree.Head;
                while (!tree.HasBeenTotallyVisited())
                {
                    var digits = new List<Digit>();

                    while (node.HasChild())
                    {
                        digits.Add(node.Value);

                        node = node.Children.First();

                        if (node.Value.Weight == 1)
                        {
                            tree.RemoveOneLeaf();

                            RemoveUntilFindChild(node);
                            break;
                        }
                    }

                    digits.Add(node.Value);
                    node = tree.Head;

                    allCombinations.Add(digits);
                }
            }

            return allCombinations;
        }

        private static void RemoveUntilFindChild(Node<Digit> node)
        {
            if (node.Parent == null)
            {
                return;
            }

            node.Parent.Remove(node);

            if (!node.Parent.HasChild())
            {
                RemoveUntilFindChild(node.Parent);
            }
        }

        private static IEnumerable<Tree<Digit>> BuildTrees(IReadOnlyList<DigitComputation> digitComputations)
        {
            var firstDigit = digitComputations[0];
            var numericsOf1StDigit = firstDigit.Numerics.ToArray();

            var trees = new List<Tree<Digit>>(firstDigit.Numerics.Count);

            for (var i = 0; i < firstDigit.Numerics.Count; i++)
            {
                var head = new Node<Digit>(new Digit(numericsOf1StDigit[i], firstDigit.ChecksumWeight), null);
                var tree = new Tree<Digit>(head);

                var currentNodes = new[] {head};

                for (var j = 1; j < digitComputations.Count; j++)
                {
                    var currentLevel = digitComputations[j];

                    var accumulation = new List<Node<Digit>>();
                    foreach (var node in currentNodes)
                    {
                        var toBeAdded = currentLevel.Numerics.Select(x => new Node<Digit>(new Digit(x, currentLevel.ChecksumWeight), node)).ToList();
                        node.AddChildren(toBeAdded);

                        accumulation.AddRange(toBeAdded);
                    }

                    currentNodes = accumulation.ToArray();
                    if (currentLevel.ChecksumWeight == 1)
                    {
                        //last level
                        tree.SetTotalLeafsNumber(currentNodes.Length);
                    }
                }

                trees.Add(tree);
            }

            return trees;
        }

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
            var computeCombinations = ComputeDigitCombinations();

            foreach (var computeCombination in computeCombinations)
            {
                var one = string.Join(string.Empty, computeCombination.Select(x => { return x.Numeric.ToString(); }));

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
            _checksum = DigitComputations.Sum(d => d.ChecksumWeight * d.Numerics.Single()) % 11;
            return _checksum == 0;
        }

        private bool IsIll()
        {
            return DigitComputations.Any(x => x.IsIllegal);
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
    }
}