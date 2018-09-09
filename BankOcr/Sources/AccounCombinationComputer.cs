using System.Collections.Generic;
using System.Linq;

namespace BankOcr.Sources
{
    public class AccounCombinationComputer
    {
        public AccounCombinationComputer(Account account)
        {
            this.DigitGroup =  account.DigitsValues.Select(x => new DigitGroup(x)).ToList();
        }

        public IReadOnlyList<DigitGroup> DigitGroup { get; private set; }

        public IEnumerable<Account> ComputeCombinations()
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

        private List<List<Digit>> ComputeDigitCombinations()
        {
            var trees = BuildTrees(DigitGroup);
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

                        if (node.Value.ChecksumWeight == 1)
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

        private static IEnumerable<Tree<Digit>> BuildTrees(IReadOnlyList<DigitGroup> digitComputations)
        {
            var firstDigit = digitComputations[0];
            int[] numericsOf1StDigit = firstDigit.Numerics.ToArray();

            var trees = new List<Tree<Digit>>(firstDigit.Numerics.Count);

            for (var i = 0; i < firstDigit.Numerics.Count; i++)
            {
                var head = new Node<Digit>(new Digit(numericsOf1StDigit[i], firstDigit.ChecksumWeight), null);
                var tree = new Tree<Digit>(head);

                var currentNodes = new[] { head };

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
    }
}