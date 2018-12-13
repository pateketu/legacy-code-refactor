using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class ChapsPayment : IPaymentType
    {
        private readonly IAccountFlag _accountFlag;

        public ChapsPayment(IAccountFlag accountFlag)
        {
            _accountFlag = accountFlag;
        }
        public bool IsAvailable(Account account, decimal requestedAmount)
        {
            return _accountFlag.HasFlag(account, AllowedPaymentSchemes.Chaps) 
                   && account.Status == AccountStatus.Live;
        }

        public PaymentScheme Type => PaymentScheme.Chaps;
    }
}