using ClearBank.DeveloperTest.ConfigSettings;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Contracts;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validations;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ServicesTests
{
    public class PaymentServiceTests
    {
        private PaymentService _systemUnderTest;
        private readonly Mock<IAccountFactory> _mockAccountFactory = new();
        private readonly Mock<ITransactionProcessor> _mockTransactionProcessor = new();
        private readonly Mock<IAccountDataStore> _mockAccountDataStore = new();
        private readonly Mock<IAppSettingsConfiguration> _mockConfiguration = new();
        private readonly Mock<IPaymentValidator> _mockValidator = new();
        private readonly Account _account = new()
        {
            AccountNumber = "123",
            Balance = 1000m,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps
        };

        public PaymentServiceTests()
        {
            _mockConfiguration.Setup(c => c.DataStoreType).Returns("Primary");
            _mockAccountFactory.Setup(f => f.CreateAccount(It.IsAny<string>())).Returns(_mockAccountDataStore.Object);
            _systemUnderTest = CreatePaymentService();
        }

        [Fact]
        public void MakePayment_WhenCalled_ShouldNotThrowException()
        {
            Action act = () => _systemUnderTest.MakePayment(new MakePaymentRequest());
            act.Should().NotThrow();
        }

        [Fact]
        public void MakePayment_WhenCalled_ShouldReturnTheResultWithTheCorrectType()
        {
            var result = _systemUnderTest.MakePayment(new MakePaymentRequest());

            result.Should().NotBeNull().And.BeOfType<MakePaymentResult>();
        }

        [Fact]
        public void MakePayment_WhenCalledAndRequestIsNull_ShouldReturnFailure()
        {
            var result = _systemUnderTest.MakePayment(null);

            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }

        [Fact]
        public void MakePayment_WhenAccountDoesNotExist_ShouldReturnFailure()
        {
            var notFoundRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = "notfound",
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = _systemUnderTest.MakePayment(notFoundRequest);

            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }

        [Fact]
        public void MakePayment_WhenCalledAndThereIsNoValidatorForScheme_ShouldReturnFailure()
        {
            var request = new MakePaymentRequest { DebtorAccountNumber = "123" };
            _mockAccountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(_account);

            var result = _systemUnderTest.MakePayment(request);

            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }

        [Fact]
        public void MakePayment_WhenCalledAndValidatorReturnsFalse_ShouldReturnFailure()
        {
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123",
                PaymentScheme = PaymentScheme.Bacs
            };
            _mockAccountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(_account);
            var validatorMock = CreateValidator(PaymentScheme.Bacs, false);
            _systemUnderTest = CreatePaymentService(validatorMock.Object);

            var result = _systemUnderTest.MakePayment(request);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
                _mockTransactionProcessor.Verify(p => p.Process(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<IAccountDataStore>()), Times.Never);
            }
        }

        [Fact]
        public void MakePayment_WhenCalledAndValidatorReturnsTrue_ShouldReturnSuccess()
        {
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123",
                PaymentScheme = PaymentScheme.Bacs,
                Amount = 100m
            };
            _mockAccountDataStore.Setup(ds => ds.GetAccount(It.IsAny<string>())).Returns(_account);
            var validatorMock = CreateValidator(PaymentScheme.Bacs, true);
            _systemUnderTest = CreatePaymentService(validatorMock.Object);

            var result = _systemUnderTest.MakePayment(request);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new MakePaymentResult { Success = true });
                _mockTransactionProcessor.Verify(p => p.Process(_account, 100m, _mockAccountDataStore.Object), Times.Once);
            }
        }

        private PaymentService CreatePaymentService(params IPaymentValidator[] validators)
        {
            return new PaymentService(
                _mockAccountFactory.Object,
                validators?.ToList() ?? [],
                _mockConfiguration.Object,
                _mockTransactionProcessor.Object
            );
        }

        private Mock<IPaymentValidator> CreateValidator(PaymentScheme scheme, bool isValid)
        {

            _mockValidator.Setup(v => v.PaymentScheme).Returns(scheme);
            _mockValidator.Setup(v => v.Validate(_account, It.IsAny<MakePaymentRequest>())).Returns(isValid);
            return _mockValidator;
        }
    }
}
