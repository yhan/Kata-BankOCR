namespace BankOcr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct Digit 
    {
        public int ChecksumWeight { get; }

        private readonly string _header;

        private readonly string _body;

        private readonly string _footer;

        private readonly HeaderReader _headerReader;
        private readonly BodyReader _bodyReader;
        private readonly FooterReader _footerReader;
        private readonly int? _numeric;
        private readonly string _digitInOneLine;


        public Digit(string[] lines, int checksumWeight = 0) : this()
        {
            ChecksumWeight = checksumWeight;

            this._header = lines[0];
            this._body = lines[1];
            this._footer = lines[2];

            this._headerReader = new HeaderReader();
            this._bodyReader = new BodyReader();
            this._footerReader = new FooterReader();
            _digitInOneLine = $"{_header}{_body}{_footer}".Replace(@"\r\n", "");

            _numeric = InitializeValue();

        }

        public Digit(int numeric, int weight) : this()
        {
            _numeric = numeric;
            ChecksumWeight = weight;
        }

        public string GetDigitAsStringInOneLine()
        {
            return _digitInOneLine;
        }

        public string GetNumericAsString()
        {
            var numeric = GetNumeric();
            return numeric == null ? "?" : numeric.ToString();
        }

        public int? GetNumeric()
        {
            return _numeric;
        }

        private int? InitializeValue()
        {
            HashSet<int> candidates1 = this._headerReader.Read(this._header);
            var candidates2 = this._bodyReader.Read(this._body);
            var candidates3 = this._footerReader.Read(this._footer);

            candidates1.IntersectWith(candidates2);
            candidates1.IntersectWith(candidates3);

            return candidates1.Count == 1 ? candidates1.Single() : default(int?);
        }

        public override string ToString()
        {
            return GetNumericAsString();
        }

        public bool IsIllegal => _numeric == null;


        public IEnumerable<int> GetCompensatedNumerics()
        {
            var digitCompensator = new DigitCompensator();
            IEnumerable<int> compensated = digitCompensator.Compensate(_digitInOneLine);

            return compensated;
        }
    }
}