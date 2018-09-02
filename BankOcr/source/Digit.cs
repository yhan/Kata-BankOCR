namespace BankOcr.source
{
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

        public string AsString()
        {
            return Numeric.ToString();
        }

        public override string ToString()
        {
            return $"[{Numeric.ToString()} w={Weight}]";
        }
    }
}