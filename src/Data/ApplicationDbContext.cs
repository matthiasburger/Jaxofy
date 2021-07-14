using System;
using Jaxofy.Data.Models;
using Jaxofy.Models.Settings;
using Jaxofy.Services.Configuration;
using Jaxofy.Services.Environment;
using Jaxofy.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jaxofy.Data
{
    /// <summary>
    /// This is an <see cref="ApplicationDbContext"/> factory class specifically for EF Core to use during execution of the <c>ef</c> CLI commands.<para> </para>
    /// Every other context should use the database context provided/injected by the DI container!
    /// </summary>
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private class Argon2TestData : IOptionsMonitor<Argon2HashingParameters>
        {
            public Argon2HashingParameters CurrentValue { get; } = new();
            public Argon2HashingParameters Get(string name) => null;
            public IDisposable OnChange(Action<Argon2HashingParameters, string> listener) => null;
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationService configurationService = new ConfigurationService();
            IConfiguration configuration = configurationService.GetPlatformAgnosticConfig();

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(configuration["DasTeamRevolutionSqlServerConnectionString"]);

            return new ApplicationDbContext(optionsBuilder.Options, new LoggerFactory(),
                new EnvironmentDiscovery(),
                new PasswordHashingArgon2(new Argon2TestData()));
        }
    }

    /// <summary>
    /// EF Core database context for interacting with the domain model.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Track> Tracks { get; set; }
        
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnvironmentDiscovery _environmentDiscovery;
        private readonly IPasswordHashing _passwordHashing;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory,
            IEnvironmentDiscovery environmentDiscovery, IPasswordHashing passwordHashing) : base(options)
        {
            _loggerFactory = loggerFactory;
            _environmentDiscovery = environmentDiscovery;
            _passwordHashing = passwordHashing;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}