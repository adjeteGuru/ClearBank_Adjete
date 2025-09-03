using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    public class TransactionProcessorTests
    {
        private readonly TransactionProcessor _systemUnderTest;
        private readonly Mock<IAccountDataStore> _mockAccountDataStore = new();

        public TransactionProcessorTests()
        {
            _systemUnderTest = new TransactionProcessor();
        }

        [Fact]
        public void Process_WhenCalled_ShouldNotThrowException()
        {
            var account = new Account { Balance = 100m };
            decimal amountToDeduct = 50m;

            Action act = () => _systemUnderTest.Process(account, amountToDeduct, _mockAccountDataStore.Object);
            act.Should().NotThrow();
        }

        [Fact]
        public void Process_WhenCalled_ShouldDeductAmountFromAccountBalance()
        {
            var account = new Account { Balance = 100m };
            decimal amountToDeduct = 50m;

            _systemUnderTest.Process(account, amountToDeduct, _mockAccountDataStore.Object);

            account.Balance.Should().Be(50m);
        }

        [Fact]
        public void Process_WhenCalledAndBalanceIsDeduced_ShouldInvokeUpdateAccountOnAccountDataStore()
        {
            var account = new Account { Balance = 100m };
            decimal amountToDeduct = 50m;

            _systemUnderTest.Process(account, amountToDeduct, _mockAccountDataStore.Object);

            _mockAccountDataStore.Verify(ds => ds.UpdateAccount(account), Times.Once);
        }

        [Fact]
        public void Process_WhenCalledAndBalanceIsDeduced_ShouldAllowNegativeBalance()
        {
            var account = new Account
            {
                AccountNumber = "456",
                Balance = 10m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var amount = 20m;
        
            _systemUnderTest.Process(account, amount, _mockAccountDataStore.Object);

            account.Balance.Should().Be(-10m);
        }
    }
}
