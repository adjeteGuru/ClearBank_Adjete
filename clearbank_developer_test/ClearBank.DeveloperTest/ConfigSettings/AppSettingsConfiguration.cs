using System.Configuration;

namespace ClearBank.DeveloperTest.ConfigSettings
{
    public class AppSettingsConfiguration : IAppSettingsConfiguration
    {
        private const string DataStoreTypeKey = "DataStoreType";
        public string DataStoreType => ConfigurationManager.AppSettings[DataStoreTypeKey];
    }
}
