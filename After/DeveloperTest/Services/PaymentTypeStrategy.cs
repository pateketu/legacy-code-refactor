using System.Collections.Generic;
using System.Linq;
using DeveloperTest.Types;

namespace DeveloperTest.Services
{
    public class PaymentTypeStrategy: IPaymentTypeStrategy
    {
        private readonly IEnumerable<IPaymentType> _payementTypes;

        public PaymentTypeStrategy(IEnumerable<IPaymentType> payementTypes)
        {
            _payementTypes = payementTypes;
        }
        public IPaymentType Get(PaymentScheme paymentScheme)
        {
            IPaymentType paymentType = this._payementTypes.FirstOrDefault(p => p.Type == paymentScheme);

            Guard.AgainstNull(paymentType, $"PaymentType for Scheme {paymentScheme.ToString()} is not registered.");

            return paymentType;
        }
    }
}
