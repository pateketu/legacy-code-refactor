using DeveloperTest.Types;

namespace DeveloperTest.Data
{
    public interface IDataStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
        string Type { get; }
    }
}