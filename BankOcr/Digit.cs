namespace BankOcr
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Digit
    {
        public int ChecksumWeight { get; }

        private readonly string _header;

        private readonly string _body;

        private readonly string _footer;

        private readonly HeaderReader headerReader;
        private readonly BodyReader bodyReader;
        private readonly FooterReader _footerReader;
        private readonly int? _value;
        private readonly string _digitInOneLine;


        public Digit(string[] lines, int checksumWeight = 0)
        {
            ChecksumWeight = checksumWeight;

            this._header = lines[0];
            this._body = lines[1];
            this._footer = lines[2];

            this.headerReader = new HeaderReader();
            this.bodyReader = new BodyReader();
            this._footerReader = new FooterReader();

            _value = InitializeValue();

            _digitInOneLine = $"{_header}{_body}{_footer}".Replace(@"\r\n", "");
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
            return _value;
        }

        private int? InitializeValue()
        {
            HashSet<int> candidates1 = this.headerReader.Read(this._header);
            var candidates2 = this.bodyReader.Read(this._body);
            var candidates3 = this._footerReader.Read(this._footer);

            candidates1.IntersectWith(candidates2);
            candidates1.IntersectWith(candidates3);

            return candidates1.Count == 1 ? candidates1.Single() : default(int?);
        }

        public override string ToString()
        {
            return GetNumericAsString();
        }

        public bool IsIllegal => _value == null;


        public IEnumerable<int> GetCorrectedNumeric()
        {
            if (IsIllegal)
            {
                var digitCompensator = new DigitCompensator();
                IEnumerable<int> compensated = digitCompensator.Compensate(_digitInOneLine);

                return compensated;
            }

            return new[] { _value.Value };
        }
    }

    public class DigitCompensator
    {
        private readonly Dictionary<int, string> _referential = new Dictionary<int, string>()
        {
            [0] = "",
            [1] = "     |  |",
            [2] = "",
            [3] = "",
            [4] = "",
            [5] = "",
            [6] = "",
            [7] = "",
            [8] = "",
            [9] = "",
        };

        internal IEnumerable<int> Compensate(string toCompensate)
        {
            foreach (var numericAndString in _referential)
            {
                var numeric = numericAndString.Key;
                string referential = numericAndString.Value;
                if (CanMatch(toCompensate, referential))
                {
                    yield return numeric;
                }
            }
        }

        /// <summary>
        /// a digit in one line has 3*3=9 chars, if <paramref name="toCompensate"/> and <paramref name="referential"/> perfectly match, return true.
        /// if only one char does not match between the two, matching degree is 8.
        /// That's to say, with add or removing a bar or underscore, <paramref name="toCompensate"/> can be compensated to <paramref name="referential"/>
        /// </summary>
        /// <param name="toCompensate"></param>
        /// <param name="referential"></param>
        /// <returns></returns>
        private bool CanMatch(string toCompensate, string referential)
        {
            if (toCompensate == referential)
            {
                return true;
            }

            int matchingDegree = 0;

            for (int i = 0; i < referential.Length; i++)
            {
                if (toCompensate[i] == referential[i])
                {
                    matchingDegree++;
                }
            }

            return matchingDegree == 8;
        }
    }
}