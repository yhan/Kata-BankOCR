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

        private readonly string _illegal = "?";

        private readonly BodyReader bodyReader;

        private readonly HeaderReader headerReader;

        public DigitComputation(string[] lines, int checksumWeight = 0)
        {
            ChecksumWeight = checksumWeight;

            _header = lines[0];
            _body = lines[1];
            _bottom = lines[2];

            headerReader = new HeaderReader();
            bodyReader = new BodyReader();
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

        public bool IsIllegal => GetNumericAsString() == _illegal;

        public Queue<int> AsQueue()
        {
            return new Queue<int>(Numerics);
        }

        public string GetNumericAsString()
        {
            var value = GetNumericPossibilities();
            if (value.Count == 1)
            {
                return value.Single().ToString();
            }

            return _illegal;
        }

        private HashSet<int> GetNumericPossibilities()
        {
            var candidates = headerReader.Read(_header);
            var candidates2 = bodyReader.Read(_body);
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