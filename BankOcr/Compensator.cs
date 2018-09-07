namespace BankOcr
{
    using System.Collections.Generic;

    internal class Compensator
    {
        internal IList<int> Convert(Digit digit)
        {
            var candidates = new List<int>();
            var replaceAscii = '_';

            for (var i = 0; i < digit.Header.Length; i++)
            {
                var header = digit.Header.ToCharArray();
                if (header[i] == ' ')
                {
                    header[i] = replaceAscii;
                    digit.ReplacedHeader = new string(header);
                    try
                    {
                        var numericAscii = digit.GetNumeric();
                        if (numericAscii.HasValue) candidates.Add(numericAscii.Value);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            digit.ReplacedHeader = null;
            var replaceAscii2 = '|';
            for (var i = 0; i < digit.Bottom.Length; i++)
            {
                var bottom = digit.Bottom.ToCharArray();
                if (bottom[i] == ' ')
                {
                    bottom[i] = replaceAscii2;
                    digit.ReplacedBottom = new string(bottom);
                    try
                    {
                        var numericAscii = digit.GetNumeric();
                        if (numericAscii.HasValue) candidates.Add(numericAscii.Value);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return candidates;
        }
    }
}