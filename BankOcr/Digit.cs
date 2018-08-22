namespace BankOcr
{
    using System;
    using System.Linq;

    public class Digit
    {
        public int ChecksumWeight { get; }

        private readonly string _header;

        private readonly string _body;

        private readonly string _bottom;

        private readonly HeaderReader headerReader;
        private readonly BodyReader bodyReader;
        private readonly FooterReader _footerReader;
        
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

        internal int GetNumeric()
        {
            var candidates1 = this.headerReader.Read(this._header);
            var candidates2 = this.bodyReader.Read(this._body);
            var candidates3 = this._footerReader.Read(this._bottom);

            candidates1.IntersectWith(candidates2);
            candidates1.IntersectWith(candidates3);

            if (candidates1.Count != 1)
            {
                throw new ApplicationException("Somewhere not correctly implemented");
            }

            return candidates1.Single();
        }
    }
}