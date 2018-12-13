using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    /*
     * Another Approach is to not individually unit testing IPaymentType and IAccountFlag implementation
     * instead carry E2E like tests like below which covers automatic testing of logic different classes
     */
    [TestClass]
    [Ignore] // Remove Ignore to See Alt Approach test running
    public class PaymentServiceTestsAltApproach
    {
        private IPaymentService _target;
        private const int BacsAndChapsFlag = (int) AllowedPaymentSchemes.Bacs + (int) AllowedPaymentSchemes.Chaps;
        private const int BacsAndChapsAndFasterPaymentFlag = (int) AllowedPaymentSchemes.Bacs + (int)AllowedPaymentSchemes.Chaps + (int)AllowedPaymentSchemes.FasterPayments;
        private const int CryptoPaymentFlag = 0;

        [TestMethod]    
        public void Should_BeUnSuccessful_IfAccountIsNotFound()
        {
            // Arrange
            _target = MockAccountAndGet(null);

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = PaymentScheme.Bacs, Amount = 100 });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
        public void Should_BeSuccessful_WhenRequestingAPaymentSchemeThatIsAllowedByAccount(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme requestedPaymentScheme)
        {
            // Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = allowedPaymentScheme, Status = AccountStatus.Live, Balance = 1000 }); ;

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedPaymentScheme, Amount = 100});

            //assert
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        [DataRow(BacsAndChapsAndFasterPaymentFlag, PaymentScheme.Bacs)]
        [DataRow(BacsAndChapsAndFasterPaymentFlag, PaymentScheme.Chaps)]
        [DataRow(BacsAndChapsAndFasterPaymentFlag, PaymentScheme.FasterPayments)]
        public void Should_BeSuccessful_WhenAnAccountSupportsMultiplePaymentTypesAndAnAllowedSchemeIsRequested(int allowedPaymentScheme, PaymentScheme requestedScheme)
        {
            // Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = (AllowedPaymentSchemes)allowedPaymentScheme, Status = AccountStatus.Live, Balance = 1000 }); ;

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedScheme, Amount = 100 });

            //assert
            Assert.IsTrue(result.Success);
        }


        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
        public void Should_BeUnsuccessful_WhenRequestingAPaymentSchemeThatIsNotAllowedByAccount(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme requestedPaymentScheme)
        {
            //Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = allowedPaymentScheme });
        
            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedPaymentScheme });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
       [DataRow(BacsAndChapsFlag, PaymentScheme.FasterPayments)]
        [DataRow(CryptoPaymentFlag, PaymentScheme.Bacs)]
        public void Should_BeUnSuccessful_WhenAnAccountSupportsMultiplePaymentTypesAndAnNotAllowedSchemeIsRequested(int allowedPaymentScheme, PaymentScheme requestedScheme)
        {
            // Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = (AllowedPaymentSchemes)allowedPaymentScheme, Status = AccountStatus.Live, Balance = 1000 }); ;

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedScheme, Amount = 100 });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Should_BeUnSuccessful_WhenRequestingChapsPaymentSchemeAndAccountIsDisabled()
        {
            // Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Disabled });

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void Should_BeUnSuccessful_WhenRequestingFasterPaymentSchemeAndAccountBalanceIsInsufficient()
        {
            // Arrange
            _target = MockAccountAndGet(new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 100 });

            //act
            var result = _target.MakePayment(new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments, Amount = 1000 });

            //assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments)]
        public void Should_ReduceBalance_WhenRequestingAllowedPayment(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme requestedPaymentScheme)
        {
            // Arrange
            decimal balance = 1000;
            decimal amount = 100;
            decimal expectedRemainingBalance = balance - amount;
            var account = new Account() { AllowedPaymentSchemes = allowedPaymentScheme, Balance = balance };
            _target = MockAccountAndGet(account);

            //act
            _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedPaymentScheme, Amount = amount });

            //assert
            Assert.AreEqual(expectedRemainingBalance, account.Balance);
        }

        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
        public void Should_NotReduceBalance_WhenRequestingAPaymentSchemeThatIsNotAllowedByAccount(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme requestedPaymentScheme)
        {
            //Arrange
            decimal balance = 1000;
            var account = new Account() {AllowedPaymentSchemes = allowedPaymentScheme, Balance = balance};
            _target = MockAccountAndGet(account);
            
            //act
            _target.MakePayment(new MakePaymentRequest() { PaymentScheme = requestedPaymentScheme, Amount = 100});

            //assert
            Assert.AreEqual(account.Balance, balance);
        }

        [TestMethod]
        public void Should_NotReduceBalance_WhenRequestingChapsPaymentSchemeAndAccountIsDisabled()
        {
            // Arrange
            decimal balance = 1000;
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Balance = balance };
            _target = MockAccountAndGet(account);
            
            //act
            _target.MakePayment(new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments, Amount = 100});

            //assert
            Assert.AreEqual(account.Balance, balance);
        }

        [TestMethod]
        public void Should_NotReduceBalance_WhenRequestingFasterPaymentSchemeAndAccountBalanceIsInsufficient()
        {
            // Arrange
            decimal balance = 100;
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = balance };
            _target = MockAccountAndGet(account);
            
            //act
            _target.MakePayment(new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments, Amount = 1000 });

            //assert
            Assert.AreEqual(account.Balance, balance);
        }

        private IPaymentService MockAccountAndGet(Account account)
        {
            return PaymentServiceBuilder.Builder().UseRealPaymentTypeStrategy().MockWith(account).Build();
        }

    }

    
}
