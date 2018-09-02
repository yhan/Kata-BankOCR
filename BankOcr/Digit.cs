namespace BankOcr
{
    using System.Collections.Generic;
    using System.Linq;

    public class Digit
    {
        private readonly string _body;

        private readonly string _bottom;

        private readonly FooterReader _footerReader;

        private readonly string _header;

        private readonly string _illegal = "?";

        private readonly BodyReader bodyReader;

        private readonly HeaderReader headerReader;

        public Digit(string[] lines, int checksumWeight = 0)
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

        public HashSet<int> Numerics { get; }

        public int ChecksumWeight { get; }

        public bool IsIllegal
        {
            get
            {
                return GetNumericAsString() == _illegal;
            }
        }

        public string GetNumericAsString()
        {
            HashSet<int> value = GetNumericPossibilities();
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
    }
}