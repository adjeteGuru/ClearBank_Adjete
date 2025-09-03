using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validations;
using FluentAssertions;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ValidationsTests
{
    public class FasterPaymentValidatorTests
    {
        private readonly FasterPaymentValidator _systemUnderTest;
        private readonly MakePaymentRequest _request = new();
        public FasterPaymentValidatorTests()
        {
            _systemUnderTest = new FasterPaymentValidator();
        }

        [Fact]
        public void Validate_WhenCalled_ShouldNotThrowException()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = new MakePaymentRequest();

            Action act = () => _systemUnderTest.Validate(account, request);
            act.Should().NotThrow();
        }

        [Fact]
        public void Validate_WhenRequestIsNull_ShouldReturnFalse()
        {
            var account = new Account();
            var result = _systemUnderTest.Validate(account, null);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountIsNull_ShouldReturnFalse()
        {
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(null, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountDoesNotAllowFasterPayments_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 200
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountBalanceIsInsufficient_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 50
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountIsValid_ShouldReturnTrue()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 200
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeTrue();
        }
        [Fact]
        public void Validate_WhenAccountAllowsFasterPaymentsButBalanceIsZero_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 0
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountAllowsFasterPaymentsAndBalanceEqualsRequestAmount_ShouldReturnTrue()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenAccountAllowsFasterPaymentsAndBalanceIsGreaterThanRequestAmount_ShouldReturnTrue()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 150
            };
            _request.Amount = 100;

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeTrue();
        }
    }
}
