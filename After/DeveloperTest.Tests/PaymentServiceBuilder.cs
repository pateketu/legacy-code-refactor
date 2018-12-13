using DeveloperTest.Data;
using DeveloperTest.Services;
using DeveloperTest.Types;
using Rhino.Mocks;

namespace DeveloperTest.Tests
{
    //Helper to build a PaymentService instance which uses a mocked up Account
    public class PaymentServiceBuilder
    {
        private IDataStoreFactory _mockDataStoreFactory;
        private IPaymentTypeStrategy _paymentTypeStrategy;
        private IDataStore _mockDataStore;

        private PaymentServiceBuilder()
        {

        }

        public static PaymentServiceBuilder Builder()
        {
            return new PaymentServiceBuilder();
        }

        public PaymentServiceBuilder MockWith(Account account)
        {
            _mockDataStore = MockRepository.GenerateMock<IDataStore>();
            _mockDataStore.Stub(x => x.GetAccount(Arg<string>.Is.Anything))
                .Return(account);

            _mockDataStoreFactory = MockRepository.GenerateStub<IDataStoreFactory>();
            _mockDataStoreFactory.Stub(x => x.Get()).Return(_mockDataStore);
            return this;
        }

        public PaymentServiceBuilder UseNotAvailablePaymentTypeStrategy()
        {
            var unAvailablePaymentType = MockRepository.GenerateStub<IPaymentType>();
            unAvailablePaymentType.Stub(x => x.IsAvailable(Arg<Account>.Is.Anything, Arg<decimal>.Is.Anything))
                .Return(false);

            _paymentTypeStrategy = MockRepository.GenerateStub<IPaymentTypeStrategy>();
            _paymentTypeStrategy.Stub(x => x.Get(Arg<PaymentScheme>.Is.Anything)).Return(unAvailablePaymentType);
            return this;
        }

        public PaymentServiceBuilder UseAvailablePaymentTypeStrategy()
        {
            var availablePaymentType = MockRepository.GenerateStub<IPaymentType>();
            availablePaymentType.Stub(x => x.IsAvailable(Arg<Account>.Is.Anything, Arg<decimal>.Is.Anything))
                .Return(true);

            _paymentTypeStrategy = MockRepository.GenerateStub<IPaymentTypeStrategy>();
            _paymentTypeStrategy.Stub(x => x.Get(Arg<PaymentScheme>.Is.Anything)).Return(availablePaymentType);
            return this;
        }

        public PaymentServiceBuilder UseRealPaymentTypeStrategy()
        {
            _paymentTypeStrategy = Bootstrapper.Container.GetInstance<IPaymentTypeStrategy>();
            return this;
        }
        

        public IPaymentService Build()
        {
            return new PaymentService(_mockDataStoreFactory, _paymentTypeStrategy);
        }

        public IDataStore CurrentDataStoreUsed => _mockDataStore;

    }
}
