namespace BankOcr
{
    public class Tree<T>
    {
        public Tree(Node<T> head)
        {
            Head = head;
        }

        public Node<T> Head { get; }

        public int LeafsCount { get; private set; }

        public void SetTotalLeafsNumber(int leafsCount)
        {
            LeafsCount = leafsCount;
        }

        public void RemoveOneLeaf()
        {
            LeafsCount = LeafsCount - 1;
        }

        public bool HasBeenTotallyVisited()
        {
            return LeafsCount == 0;
        }
    }
}