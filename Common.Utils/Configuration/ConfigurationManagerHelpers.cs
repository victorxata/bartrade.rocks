using System.Configuration;

namespace Common.Utils.Configuration
{
    public static class ConfigurationManagerHelpers
    {
        public static string GetValueFromConfig(string keyName)
        {
            var value = ConfigurationManager.AppSettings[keyName];
            if (value == null)
            {
                throw new ConfigurationErrorsException($"Expecting an App.Config value for {keyName}");
            }

            return value;
        }
    }
}
