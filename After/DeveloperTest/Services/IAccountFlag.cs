using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public interface IAccountFlag
    {
        bool HasFlag(Account account, AllowedPaymentSchemes paymentScheme);
    }
}