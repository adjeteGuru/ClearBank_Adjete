using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using FluentAssertions;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    public class AccountFactoryTests
    {
        private readonly AccountFactory _systemUnderTest;

        public AccountFactoryTests()
        {
            _systemUnderTest = new AccountFactory();
        }

        [Fact]
        public void CreateAccount_WhenCalled_ShouldNotThrowException()
        {
            Action act = () => _systemUnderTest.CreateAccount(null);
            act.Should().NotThrow();
        }

        [Fact]
        public void CreateAccount_WhenCalledWithBackupDataStoreType_ShouldReturnsTheCorrectType()
        {
            string dataStoreType = "Backup";

            var result = _systemUnderTest.CreateAccount(dataStoreType);

            result.Should().BeOfType<BackupAccountDataStore>();
        }

        [Fact]
        public void CreateAccount_WhenCalledWithAnyOtherDataStoreTypeName_ShouldReturnsTheCorrectType()
        {
            string dataStoreType = "other name";

            var result = _systemUnderTest.CreateAccount(dataStoreType);

            result.Should().BeOfType<AccountDataStore>();
        }
    }
}
