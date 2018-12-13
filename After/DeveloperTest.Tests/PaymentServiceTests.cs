using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        private IPaymentService _target;

        [TestMethod]
        public void Should_BeUnSuccessful_IfAccountIsNotFound()
        {
            // Aarange
            _target = PaymentServiceBuilder.Builder().UseAvailablePaymentTypeStrategy().MockWith(null).Build();

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { Amount = 100 });

            //assert
            Assert.IsFalse(result.Success);
        }   

        [TestMethod]
        public void Should_BeUnsuccessfulWhenRequestedPaymentTypeIsNotAvailableForTheAccount()
        {
            // Aarange
            _target = PaymentServiceBuilder.Builder().UseNotAvailablePaymentTypeStrategy().MockWith(new Account()).Build();

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { Amount = 100 });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Should_ReduceBalanceOnTheAccount_WhenRequestingAllowedPaymentScheme()   
        {
            // Aarange
            decimal balance = 1000;
            decimal amount = 100;
            decimal expectedRemainingBalance = balance - amount;
            var account = new Account() { Balance = balance };
            _target = PaymentServiceBuilder.Builder().UseAvailablePaymentTypeStrategy().MockWith(account).Build();

            //act
            _target.MakePayment(new MakePaymentRequest() { Amount = amount });

            //assert
            Assert.AreEqual(expectedRemainingBalance, account.Balance);
        }

        [TestMethod]    
        public void Should_CallUpdateAccountInDataStore_WhenRequestingAllowedPaymentScheme()
        {
            // Aarange
            var account = new Account();
            var paymentServiceBuilder = PaymentServiceBuilder.Builder();
            _target = paymentServiceBuilder.UseAvailablePaymentTypeStrategy().MockWith(account).Build();

            //act
            _target.MakePayment(new MakePaymentRequest());

            //assert
            paymentServiceBuilder.CurrentDataStoreUsed.AssertWasCalled(x => x.UpdateAccount(
                Arg<Account>.Is.Equal(account)));
        }

        [TestMethod]
        public void Should_NotReduceBalanceOnTheAccount_WhenRequestingDisAllowedPaymentScheme()
        {
            // Aarange
            decimal balance = 1000;
            var account = new Account() { Balance = balance };
            _target = PaymentServiceBuilder.Builder().UseNotAvailablePaymentTypeStrategy().MockWith(account).Build();

            //act
            _target.MakePayment(new MakePaymentRequest() { Amount = 100 });

            //assert
            Assert.AreEqual(balance, account.Balance);
        }
            
        [TestMethod]
        public void Should_NotCallUpdateAccountInDataStore_WhenRequestingDisAllowedPaymentScheme()
        {
            // Aarange
            var account = new Account();
            var paymentServiceBuilder = PaymentServiceBuilder.Builder();
            _target = paymentServiceBuilder.UseNotAvailablePaymentTypeStrategy().MockWith(account).Build();

            //act
            _target.MakePayment(new MakePaymentRequest());

            //assert
            paymentServiceBuilder.CurrentDataStoreUsed.AssertWasNotCalled(x => x.UpdateAccount(
                Arg<Account>.Is.Anything));
        }
    }
}
