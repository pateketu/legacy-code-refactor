using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class FasterPaymentTests  
    {
        private FasterPayments _target;

        [TestInitialize]
        public void Setup() 
        {
            _target = new FasterPayments(Bootstrapper.Container.GetInstance<IAccountFlag>());
        }
            
        [TestMethod]
        public void Should_ReturnTrueWhenAnAccountHasSufficientBalanceAndAllowsFasterPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 1000};

            var result = _target.IsAvailable(account, 100);

            Assert.IsTrue(result);
        }   

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountllowsFasterPaymentSchemeButHasInSufficientBalance()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 100 };

            var result = _target.IsAvailable(account, 1000);

            Assert.IsFalse(result);  
        }
            
        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountDisAllowsFasterPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

            var result = _target.IsAvailable(account, 0);

            Assert.IsFalse(result);
        }

        
    }
}
