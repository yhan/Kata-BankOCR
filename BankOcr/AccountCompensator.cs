using System;
using System.Collections.Generic;
using System.Linq;

namespace BankOcr
{
    public class AccountCompensator
    {
        public IEnumerable<Account> Compensate(Account account)
        {
            if (account.IsValid())
            {
                throw new InvalidOperationException($"Account ({account.AsString()}) is already valid. No need to compensate.");
            }

            var digits = account.DigitsValues.ToArray();
            
            const int digitsLength = 9;
            for (int i = 0; i < digitsLength; i++)
            {
                var digit = digits[i];
                var compensatedNumerics = digit.GetCompensatedNumerics();
                foreach (var compensatedNumeric in compensatedNumerics)
                {
                    var numerics = GetOriginalNumerics(account);

                    numerics[i] = compensatedNumeric;

                    var compensatedAccount = new Account(numerics);
                    if (compensatedAccount.IsValid())
                    {
                        yield return compensatedAccount;
                    }
                }
            }
        }

        private static int[] GetOriginalNumerics(Account account)
        {
            return account.DigitsValues.Select(x => x.GetNumeric().Value).ToArray();
        }
    }
}