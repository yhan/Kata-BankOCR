namespace BankOcr
{
    using System.Collections.Generic;

    public interface IOcrLineReader
    {
        HashSet<int> Read(string input);
    }
}