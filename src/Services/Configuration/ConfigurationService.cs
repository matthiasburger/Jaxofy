using System;
using System.IO;
using System.Net.NetworkInformation;
using IronSphere.Extensions;
using Microsoft.Extensions.Configuration;

namespace Jaxofy.Services.Configuration
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
            configurationBuilder.AddJsonFile(_ensureDeviceConfigExists(), false, true);
            configurationBuilder.AddJsonFile("secrets.json", true, true);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(cliArgs ?? Array.Empty<string>());
            
            return configurationBuilder.Build();
        }
        
        private static string _getComputerName()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            return computerProperties.HostName;
        }
        
        private static string _ensureDeviceConfigExists()
        {
            string computerName = _getComputerName();
            string path = $@"appsettings.{computerName}.json";

            if (File.Exists(path)) 
                return path;
            
            using FileStream fileStream = File.Create(path);
            fileStream.Write($"{{{System.Environment.NewLine}}}".GetBytes());

            return path;
        }
    }
}