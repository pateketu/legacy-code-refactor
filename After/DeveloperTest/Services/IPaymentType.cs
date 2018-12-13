using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public interface IPaymentType
    {
        bool IsAvailable(Account account, decimal requestedAmount);
        PaymentScheme Type { get; }
    }
}