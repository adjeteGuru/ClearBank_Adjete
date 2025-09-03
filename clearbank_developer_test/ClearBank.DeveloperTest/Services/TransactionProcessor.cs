using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Services.Contracts;

namespace ClearBank.DeveloperTest.Services
{
    public class TransactionProcessor : ITransactionProcessor
    {
        public void Process(Account account, decimal amount, IAccountDataStore accountDataStore)
        {
            account.Balance -= amount;
            accountDataStore.UpdateAccount(account);
        }
    }
}
