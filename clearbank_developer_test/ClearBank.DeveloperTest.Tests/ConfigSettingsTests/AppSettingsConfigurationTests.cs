using ClearBank.DeveloperTest.ConfigSettings;
using FluentAssertions;
using System.Configuration;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.ConfigSettingsTests
{
    public class AppSettingsConfigurationTests
    {
        private readonly AppSettingsConfiguration _systemUnderTest;

        public AppSettingsConfigurationTests()
        {
            _systemUnderTest = new AppSettingsConfiguration();
        }

        [Fact]
        public void DataStoreType_WhenKeyExists_ShouldReturnCorrectValue()
        {
            ConfigurationManager.AppSettings["DataStoreType"] = "TestStore";

            var result = _systemUnderTest.DataStoreType;

            result.Should().Be("TestStore");
        }
    }
}
