using System.Collections.Generic;

namespace BankOcr.source
{
    public interface IOcrLineReader
    {
        HashSet<int> Read(string input);
    }
}