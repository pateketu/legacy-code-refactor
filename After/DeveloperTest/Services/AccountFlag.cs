
using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class AccountFlag : IAccountFlag
    {
        public bool HasFlag(Account account, AllowedPaymentSchemes paymentScheme)
        {
            return account.AllowedPaymentSchemes.HasFlag(paymentScheme);
        }
    }
}