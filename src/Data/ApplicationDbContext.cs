using System;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Settings;
using DasTeamRevolution.Services.Configuration;
using DasTeamRevolution.Services.Environment;
using DasTeamRevolution.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DasTeamRevolution.Data
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
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<ClientUserSetting> ClientUserPermissions { get; set; }
        public DbSet<ClientGroup> ClientGroups { get; set; }
        public DbSet<ClientHeader> ClientHeaders { get; set; }
        public DbSet<ClientHeaderAdmin> ClientHeaderAdmins { get; set; }
        public DbSet<ClientEmployeeProfile> ClientProfiles { get; set; }
        public DbSet<ClientSupplier> ClientSuppliers { get; set; }
        public DbSet<JobProfile> JobProfiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierUser> SupplierUsers { get; set; }
        public DbSet<SupplierHeader> SupplierHeaders { get; set; }
        public DbSet<SupplierUserSetting> SupplierUserPermissions { get; set; }
        public DbSet<PoolEmployee> PoolEmployees { get; set; }
        public DbSet<PoolEmployeeDocument> PoolEmployeeDocuments { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyStateHistory> VacancyStates { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<ProposalDocument> ProposalDocuments { get; set; }
        public DbSet<ProposalStateHistory> ProposalStates { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }
        public DbSet<TimeRecordStateHistory> TimeRecordStates { get; set; }
        public DbSet<RecordCreation> RecordCreations { get; set; }
        public DbSet<RecordModification> RecordModifications { get; set; }
        public DbSet<PostalAddress> PostalAddresses { get; set; }

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