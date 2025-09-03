using ClearBank.DeveloperTest.ConfigSettings;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Services.Contracts;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(
        IAccountFactory accountFactory,
        IEnumerable<IPaymentValidator> paymentValidators,
        IAppSettingsConfiguration configuration,
        ITransactionProcessor transactionProcessor) : IPaymentService
    {
        private readonly Dictionary<PaymentScheme, IPaymentValidator> _paymentValidators = paymentValidators
                ?.Where(v => v != null)
                .ToDictionary(v => v.PaymentScheme)
                ?? new Dictionary<PaymentScheme, IPaymentValidator>();

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = true };

            if (request is null)
            {
                return new MakePaymentResult { Success = false };
            }

            var accountDataStore = accountFactory.CreateAccount(configuration.DataStoreType);
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (account is null)
            {
                return new MakePaymentResult { Success = false };
            }

            if (!_paymentValidators.TryGetValue(request.PaymentScheme, out var paymentValidator))
            {
                return new MakePaymentResult { Success = false };
            }

            var isValid = paymentValidator.Validate(account, request);

            if (!isValid)
            {
                return new MakePaymentResult { Success = false };
            }

            transactionProcessor.Process(account, request.Amount, accountDataStore);

            return result;
        }
    }
}
