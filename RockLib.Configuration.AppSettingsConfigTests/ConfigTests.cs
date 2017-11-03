using System;
using Xunit;

namespace RockLib.Configuration.AppSettingsConfigTests
{
    public class ConfigTests
    {
        static ConfigTests()
        {
            // Set the environment variable before reading configs.
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("ROCKLIB_ENVIRONMENT", "Staging");
            Environment.SetEnvironmentVariable("AppSettings:RockLib.Environment", "Prod");
        }

        [Fact(DisplayName = "AppSettingsConfigTests: IsDefault is true by default.")]
        public void IsDefaultIsTrueByDefault()
        {
            Assert.True(Config.IsDefault);
        }

        [Fact(DisplayName = "AppSettingsConfigTests: 'appsettings.json' is used by default.")]
        public void FileConfigIsUsedByDefault()
        {
            Assert.Equal("201740", Config.AppSettings["ApplicationId"]);
        }

        [Fact(DisplayName = "AppSettingsConfigTests: Environment variables override values from 'appsettings.json'.")]
        public void EnvironmentVariablesOverrideFileConfig()
        {
            // Note that the "AppSettings:RockLib.Environment" environment variable was
            // set in the static constructor with a value of "Prod".
            Assert.Equal("Prod", Config.AppSettings["RockLib.Environment"]);
        }

        [Fact(DisplayName = "AppSettingsConfigTests: IsLocked is true after the ConfigurationRoot property has been accessed.")]
        public void IsLockedIsTrueAfterConfigurationRootHasBeenAccessed()
        {
            // Accessing ConfigurationRoot causes IsLocked to be true.
            var root = Config.Root;

            Assert.True(Config.IsLocked);
        }

        [Fact(DisplayName = "AppSettingsConfigTests: Changing environment variable values when IsLocked is true has no effect.")]
        public void EnvironmentVariablesDoNotOverrideAfterLocked()
        {
            // Accessing ConfigurationRoot causes IsLocked to be true.
            var root = Config.Root;

            // This is the same environment variable that we successfully changed in the static constructor.
            // Setting it this time, however, is too late.
            Environment.SetEnvironmentVariable("AppSettings:RockLib.Environment", "Beta");

            // No effect.
            Assert.Equal("Prod", Config.AppSettings["RockLib.Environment"]);
        }

        [Fact(DisplayName = "AppSettingsConfigTests: appsettings.development.json is the loaded as FileName due to priority")]
        public void AppSettingsDevelopmentJsonIsLoaded()
        {
            Assert.Equal("appsettings.development.json", Config.AppSettings["FileName"]);
        }
    }
}
