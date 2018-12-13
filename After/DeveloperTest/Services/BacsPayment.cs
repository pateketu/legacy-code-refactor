using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class BacsPayment : IPaymentType
    {
        private readonly IAccountFlag _accountFlag;

        public BacsPayment(IAccountFlag accountFlag)
        {
            _accountFlag = accountFlag;
        }

        public bool IsAvailable(Account account, decimal requestedAmount)
        {
            return this._accountFlag.HasFlag(account, AllowedPaymentSchemes.Bacs);
        }

        public PaymentScheme Type => PaymentScheme.Bacs;
    }
}