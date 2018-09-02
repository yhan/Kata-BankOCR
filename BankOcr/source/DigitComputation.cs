using System.Collections.Generic;
using System.Linq;

namespace BankOcr.source
{
    public class DigitComputation
    {
        private readonly string _body;

        private readonly string _bottom;

        private readonly FooterReader _footerReader;

        private readonly string _header;

        private const string Illegal = "?";

        private readonly BodyReader _bodyReader;

        private readonly HeaderReader _headerReader;

        public DigitComputation(string[] lines, int checksumWeight = 0)
        {
            ChecksumWeight = checksumWeight;

            _header = lines[0];
            _body = lines[1];
            _bottom = lines[2];

            _headerReader = new HeaderReader();
            _bodyReader = new BodyReader();
            _footerReader = new FooterReader();

            Numerics = GetNumericPossibilities();
        }

        //For test
        public DigitComputation(ISet<int> numerics, int checksumWeight)
        {
            ChecksumWeight = checksumWeight;
            Numerics = numerics;
        }

        public ISet<int> Numerics { get; set; }

        public int ChecksumWeight { get; }

        public bool IsIllegal => GetNumericAsString() == Illegal;

        public string GetNumericAsString()
        {
            var value = GetNumericPossibilities();
            if (value.Count == 1)
            {
                return value.Single().ToString();
            }

            return Illegal;
        }

        private HashSet<int> GetNumericPossibilities()
        {
            var candidates = _headerReader.Read(_header);
            var candidates2 = _bodyReader.Read(_body);
            var candidates3 = _footerReader.Read(_bottom);

            candidates.IntersectWith(candidates2);
            candidates.IntersectWith(candidates3);

            return candidates;
        }


        public override string ToString()
        {
            return $"{string.Join(", ", Numerics)}"; // q={string.Join(", ", AsQueue())}
        }
    }
}