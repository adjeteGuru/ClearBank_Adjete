using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services.Contracts
{
    public interface IAccountFactory
    {
        IAccountDataStore CreateAccount(string dataStoreType);
    }
}
