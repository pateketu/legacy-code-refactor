using System.Collections.Generic;
using System.Linq;
using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class PaymentTypeStrategy: IPaymentTypeStrategy
    {
        private readonly IEnumerable<IPaymentType> _paymentTypes;

        public PaymentTypeStrategy(IEnumerable<IPaymentType> paymentTypes)
        {
            _paymentTypes = paymentTypes;
        }
        public IPaymentType Get(PaymentScheme paymentScheme)
        {
            IPaymentType paymentType = this._paymentTypes.FirstOrDefault(p => p.Type == paymentScheme);

            Guard.AgainstNull(paymentType, $"PaymentType for Scheme {paymentScheme.ToString()} is not registered.");

            return paymentType;
        }
    }
}
