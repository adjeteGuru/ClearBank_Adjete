using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validations
{
    public class BacsPaymentValidator : IPaymentValidator
    {
        public PaymentScheme PaymentScheme => PaymentScheme.Bacs;

        public bool Validate(Account account, MakePaymentRequest request)
        {
            return request != null
                 && account != null
                 && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}
