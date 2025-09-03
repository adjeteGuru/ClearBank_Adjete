using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validations;
using FluentAssertions;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ValidationsTests
{
    public class ChapsPaymentValidatorTests
    {
        private readonly ChapsPaymentValidator _systemUnderTest;
        private readonly MakePaymentRequest _request = new();
        public ChapsPaymentValidatorTests()
        {            
            _systemUnderTest = new ChapsPaymentValidator();
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
            var result = _systemUnderTest.Validate(null, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountDoesNotAllowChaps_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Status = AccountStatus.Live
            };

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountIsNotLive_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Disabled
            };

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountIsLiveAndAllowsChaps_ShouldReturnTrue()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live
            };

            var result = _systemUnderTest.Validate(account, _request);
            result.Should().BeTrue();
        }
    }
}
