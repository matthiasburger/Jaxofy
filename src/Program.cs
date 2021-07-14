using System;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using DasTeamRevolution.Services.Environment;
using DasTeamRevolution.Services.Configuration;

namespace DasTeamRevolution
{
    /// <summary>
    /// Backend application entry point.
    /// </summary>
    public static class Program
    {
        private static readonly IEnvironmentDiscovery EnvironmentDiscovery = new EnvironmentDiscovery();
        private static readonly IConfigurationService ConfigurationService = new ConfigurationService();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(ConfigurationService.GetPlatformAgnosticConfig(args))
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information($"Starting {nameof(DasTeamRevolution)} web host...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{nameof(DasTeamRevolution)} host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            if (EnvironmentDiscovery.IsDocker)
            {
                Console.WriteLine("Running inside Docker");
            }

            return WebHost
                .CreateDefaultBuilder(args)
                .UseConfiguration(ConfigurationService.GetPlatformAgnosticConfig(args))
                .UseUrls(EnvironmentDiscovery.IsDocker ? "http://*:80" : "http://localhost:8000")
                .UseStartup<Startup>()
                .UseSerilog();
        }
    }
}