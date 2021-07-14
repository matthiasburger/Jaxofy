using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DasTeamRevolution.Services.Configuration
{
    /// <inheritdoc />
    public class ConfigurationService : IConfigurationService
    {
        /// <inheritdoc />
        public IConfiguration GetPlatformAgnosticConfig(string[] cliArgs = null)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json", false, true);
            configurationBuilder.AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true);
            configurationBuilder.AddJsonFile("secrets.json", true, true);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(cliArgs ?? Array.Empty<string>());
            
            return configurationBuilder.Build();
        }
    }
}