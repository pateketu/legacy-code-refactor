using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperTest.Services;
using DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class PaymentTypeStrategyTests
    {
        private IPaymentTypeStrategy _target;

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_throwAnExceptionWhenNoPaymentTypeIsAvailable()
        {
            _target = new PaymentTypeStrategy(Enumerable.Empty<IPaymentType>());

            _target.Get(PaymentScheme.FasterPayments);
        }

        [TestMethod]
        public void Should_ReturnExpectedPaymentType()
        {
           _target = new PaymentTypeStrategy(new List<IPaymentType>(){new BacsPayment(new AccountFlag()), new ChapsPayment(new AccountFlag())});

            var paymentType = _target.Get(PaymentScheme.Chaps);
            Assert.IsInstanceOfType(paymentType, typeof(ChapsPayment));

        }
    }
}
