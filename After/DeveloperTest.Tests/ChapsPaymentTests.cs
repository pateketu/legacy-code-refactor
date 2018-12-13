using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class ChapsPaymentTests  
    {
        private ChapsPayment _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new ChapsPayment(Bootstrapper.Container.GetInstance<IAccountFlag>());
        }

        [TestMethod]
        public void Should_ReturnTrueWhenAnAccountIsLiveAndAllowsChapsPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Live};

            var result = _target.IsAvailable(account, 0);

            Assert.IsTrue(result);
        }   

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountAllowsChapsPaymentSchemeButIsDisabled()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status=AccountStatus.Disabled };

            var result = _target.IsAvailable(account, 0);

            Assert.IsFalse(result);
        }
            
        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountDisAllowsChapsPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

            var result = _target.IsAvailable(account, 0);

            Assert.IsFalse(result);
        }

        
    }
}
