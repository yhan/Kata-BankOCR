namespace BankOcr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework.Internal.Execution;

    public class Digit
    {
        public int ChecksumWeight { get; }

        private readonly string _header;

        private readonly string _body;

        private readonly string _bottom;

        private readonly HeaderReader headerReader;
        private readonly BodyReader bodyReader;
        private readonly FooterReader _footerReader;

        private readonly string _illegal = "ILL";

        public Digit(string[] lines, int checksumWeight = 0)
        {
            ChecksumWeight = checksumWeight;

            this._header = lines[0];
            this._body = lines[1];
            this._bottom = lines[2];

            this.headerReader = new HeaderReader();
            this.bodyReader = new BodyReader();
            this._footerReader = new FooterReader();
        }

        public string ReplacedHeader { get; set; }
        public string ReplacedBottom { get; set; }

        public string GetNumericAsString()
        {
            var candidates1 = this.headerReader.Read(ReplacedHeader ?? Header);
            var candidates2 = this.bodyReader.Read(_body);
            var candidates3 = this._footerReader.Read(ReplacedBottom ?? Bottom);

            candidates1.IntersectWith(candidates2);
            candidates1.IntersectWith(candidates3);

            return candidates1.Count == 1 ? candidates1.Single().ToString() : _illegal;
        }

        public int? GetNumeric()
        {
            var numericAsString = GetNumericAsString();
            if (numericAsString != _illegal)
            {
                return _forcedValue ?? int.Parse(numericAsString);
            }
            return null;
        }

        public override string ToString()
        {
            var numeric = GetNumericAsString();
            if (this.IsIllegal)
            {
                return "?";
            }

            return numeric.ToString();
        }

        public bool IsIllegal => GetNumericAsString() == _illegal;

        public string Header
        {
            get { return _header; }
        }

        public string Bottom
        {
            get { return _bottom; }
        }

        public void ForceValue(int? value)
        {
            _forcedValue = value;
        }

        private int? _forcedValue;
    }
}