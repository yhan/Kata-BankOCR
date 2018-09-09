using System;
using System.Collections.Generic;
using System.Linq;

namespace BankOcr.Sources
{
    public class AccountCompensator
    {
        public IEnumerable<Account> Compensate(Account account)
        {
            if (account.IsValid())
            {
                throw new InvalidOperationException($"Account ({account.ToString()}) is already valid. No need to compensate.");
            }

            var digits = account.DigitsValues.ToArray();
            
            const int digitsLength = 9;
            for (int i = 0; i < digitsLength; i++)
            {
                //this algo won't work if account contains illegal digit
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