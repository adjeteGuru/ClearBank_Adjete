using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validations
{
    public class ChapsPaymentValidator : IPaymentValidator
    {
        public PaymentScheme PaymentScheme => PaymentScheme.Chaps;

        public bool Validate(Account account, MakePaymentRequest request)
        {
            return request != null
                && account != null
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                && account.Status == AccountStatus.Live;
        }
    }
}
