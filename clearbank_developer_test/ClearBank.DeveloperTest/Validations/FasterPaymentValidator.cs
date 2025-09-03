using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validations
{
    public class FasterPaymentValidator : IPaymentValidator
    {
        public PaymentScheme PaymentScheme => PaymentScheme.FasterPayments;

        public bool Validate(Account account, MakePaymentRequest request)
        {
            return request != null
                 && account != null
                 && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                 && account.Balance >= request.Amount;
        }
    }
}
