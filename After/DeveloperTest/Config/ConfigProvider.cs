using System.Configuration;

namespace DeveloperTest.Config
{
    public class ConfigProvider : IConfigProvider
    {
        public string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
