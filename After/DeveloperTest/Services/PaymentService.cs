using DeveloperTest.Data;
using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStoreFactory _dataStoreFactory;
        private readonly IPaymentTypeStrategy _paymentTypeStrategy;

        public PaymentService(IDataStoreFactory dataStoreFactory, IPaymentTypeStrategy paymentTypeStrategy)
        {
            _dataStoreFactory = dataStoreFactory;
            _paymentTypeStrategy = paymentTypeStrategy;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            IDataStore dataStore = this._dataStoreFactory.Get();
            Account account = dataStore.GetAccount(request.DebtorAccountNumber);
            var result = new MakePaymentResult();
            if (account == null)
            {
                result.Success = false;
                return result;
            }

            result.Success = this._paymentTypeStrategy.Get(request.PaymentScheme).IsAvailable(account, request.Amount);

            if (result.Success)
            {
                account.Balance -= request.Amount;

                dataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
