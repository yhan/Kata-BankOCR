using System.Collections.Generic;
using System.Linq;

namespace BankOcr.Sources
{
    public class Node<T>

    {
        public Node(T t, Node<Digit> parent)
        {
            Value = t;
            Parent = parent;
        }

        public List<Node<T>> Children { get; } = new List<Node<T>>();

        public T Value { get; set; }

        public Node<Digit> Parent { get; }

        public bool Removed { get; private set; }

        public void AddChildren(IEnumerable<Node<T>> children)
        {
            Children.AddRange(children);
        }

        public override string ToString()
        {
            var childrenDump = string.Join(",", Children.Select(x => x.Value.ToString()));

            return $"{Value.ToString()}:({childrenDump})";
        }

        public bool HasChild()
        {
            return Children.Any();
        }

        public void Remove(Node<T> node)
        {
            node.Removed = true;

            Children.Remove(node);
        }
    }
}