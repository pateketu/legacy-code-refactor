using DeveloperTest;
using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class AccountFlagTests
    {
        private IAccountFlag _target;

        [TestInitialize]
        public void Setup()
        {
            _target = Bootstrapper.Container.GetInstance<IAccountFlag>();
        }

        [TestMethod]
        public void Should_ReturnTrueWhenAnAccountAllowsAPaymentScheme()
        {
            var account = new Account() {AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs};

            var result = _target.HasFlag(account, AllowedPaymentSchemes.Bacs);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountDoesNotAllowAPaymentScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

            var result = _target.HasFlag(account, AllowedPaymentSchemes.Chaps);

            Assert.IsFalse(result);
        }   

        [TestMethod]
        public void Should_ReturnTrueWhenAnAccountAllowsMultiplePaymentSchemeAndCheckIsMadeForAnAllowedScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs + (int) AllowedPaymentSchemes.Chaps };

            var result = _target.HasFlag(account, AllowedPaymentSchemes.Bacs);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountAllowsMultiplePaymentSchemeAndCheckIsMadeForADisAllowedScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs + (int)AllowedPaymentSchemes.Chaps };

            var result = _target.HasFlag(account, AllowedPaymentSchemes.FasterPayments);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_ReturnFalseWhenAnAccountAllowsMultiplePaymentSchemeAndCheckIsMadeForAnUnknownScheme()
        {
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs + (int) AllowedPaymentSchemes.Chaps + (int) AllowedPaymentSchemes.FasterPayments };
            var cryptoPaymentFlag = 666;

            var result = _target.HasFlag(account, (AllowedPaymentSchemes)cryptoPaymentFlag);

            Assert.IsFalse(result);
        }
    }
}
