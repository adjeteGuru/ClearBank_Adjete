using ClearBank.DeveloperTest.Models;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validations;
using FluentAssertions;
using System;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ValidationsTests
{
    public class BacsPaymentValidatorTests
    {
        private readonly BacsPaymentValidator _systemUnderTest;

        public BacsPaymentValidatorTests()
        {
            _systemUnderTest = new BacsPaymentValidator();
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
            var request = new MakePaymentRequest();

            var result = _systemUnderTest.Validate(null, request);

            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountDoesNotAllowBacs_ShouldReturnFalse()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = new MakePaymentRequest();

            var result = _systemUnderTest.Validate(account, request);

            result.Should().BeFalse();
        }

        [Fact]
        public void Validate_WhenAccountAllowsBacsAndRequestIsValid_ShouldReturnTrue()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = new MakePaymentRequest();

            var result = _systemUnderTest.Validate(account, request);

            result.Should().BeTrue();
        }
    }
}
