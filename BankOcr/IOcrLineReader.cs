using System.Collections.Generic;

namespace BankOcr
{
    public interface IOcrLineReader
    {
        HashSet<int> Read(string input);
    }
}