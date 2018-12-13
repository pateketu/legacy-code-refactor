using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    /*
         Albeit BacsPayment class does not currently have any logic other than calling
         AccountFlag to check, this test will help in future when BacsPayment may be updated/re-factored
    */
    [TestClass]
    public class BacsPaymentTests
    {
        private BacsPayment _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new BacsPayment(Bootstrapper.Container.GetInstance<IAccountFlag>());
        }

        [TestMethod]
        public void Should_ReturnTrueWhenAnAccountAllowsBacsPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

            var result = _target.IsAvailable(account, 0);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountDisAllowsBacsPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps };

            var result = _target.IsAvailable(account, 0);

            Assert.IsFalse(result);
        }
    }
}
