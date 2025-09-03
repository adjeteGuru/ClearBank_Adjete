using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Models;

namespace ClearBank.DeveloperTest.Services.Contracts
{
    public interface ITransactionProcessor
    {
        void Process(Account account, decimal amount, IAccountDataStore accountDataStore);
    }
}
