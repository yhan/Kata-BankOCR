namespace BankOcr
{
    internal class Compensator
    {
        private readonly string[] _readAllLines;

        private HeaderReaderWithCompensation _headerReaderWithCompensation;

        private BodyWithCompensation _bodyReaderWithCompensation;

        private FooterWithCompensation _footerReaderWithCompensation;

        private string _header;

        private string _body;

        private string _bottom;

        public Compensator(string[] lines)
        {
            _readAllLines = lines;


            this._header = lines[0];
            this._body = lines[1];
            this._bottom = lines[2];


            this._headerReaderWithCompensation = new HeaderReaderWithCompensation();
            this._bodyReaderWithCompensation = new BodyWithCompensation();
            this._footerReaderWithCompensation = new FooterWithCompensation();
        }

        internal int Convert()
        {
            var candidates1 = this._headerReaderWithCompensation.Read(_header);
            var candidates2 = this._bodyReaderWithCompensation.Read(this._body);
            var candidates3 = this._footerReaderWithCompensation.Read(this._bottom);

            candidates1.IntersectWith(candidates2);
            candidates1.IntersectWith(candidates3);


            return 1;
        }
    }
}