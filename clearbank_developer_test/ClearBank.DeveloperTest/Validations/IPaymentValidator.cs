using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validations
{
    public interface IPaymentValidator
    {
        PaymentScheme PaymentScheme { get; }
        bool Validate(Account account, MakePaymentRequest request);
    }
}
