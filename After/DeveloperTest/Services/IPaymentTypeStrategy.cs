using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public interface IPaymentTypeStrategy
    {
        IPaymentType Get(PaymentScheme paymentScheme);
    }
}