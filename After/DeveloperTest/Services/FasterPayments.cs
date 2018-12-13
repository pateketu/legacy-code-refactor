using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class FasterPayments : IPaymentType
    {
        private readonly IAccountFlag _accountFlag;

        public FasterPayments(IAccountFlag accountFlag)
        {
            _accountFlag = accountFlag;
        }

        public bool IsAvailable(Account account, decimal requestedAmount)
        {
            return _accountFlag.HasFlag(account, AllowedPaymentSchemes.FasterPayments) 
                   && account.Balance > requestedAmount;
        }

        public PaymentScheme Type => PaymentScheme.FasterPayments;
    }
}