using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services.Contracts;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountFactory : IAccountFactory
    {
        public IAccountDataStore CreateAccount(string dataStoreType)
        {
            return dataStoreType == "Backup"
                ? new BackupAccountDataStore()
                : new AccountDataStore();
        }
    }
}
