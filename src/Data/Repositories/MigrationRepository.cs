using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jaxofy.Data.Repositories
{
    public interface IMigrationRepository
    {
        Task ApplyMigration(string migration);
        Task<IEnumerable<string>> GetPendingMigrations();
        Task<IEnumerable<string>> GetAppliedMigrations();
        Task RemoveDatabase();
    }

    public class MigrationRepository<TDataContext> : IMigrationRepository where TDataContext : ApplicationDbContext
    {
        private readonly TDataContext _dataContext;

        public MigrationRepository(TDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task RemoveDatabase()
        {
            await _dataContext.Database.EnsureDeletedAsync();
        }

        public async Task ApplyMigration(string migration = null)
        {
            Task migrationTask = string.IsNullOrWhiteSpace(migration)
                ? _dataContext.Database.MigrateAsync()
                : _dataContext.GetService<IMigrator>().MigrateAsync(migration);

            await migrationTask;
        }

        public async Task<IEnumerable<string>> GetPendingMigrations() =>
            await _dataContext.Database.GetPendingMigrationsAsync();

        public async Task<IEnumerable<string>> GetAppliedMigrations() =>
            await _dataContext.Database.GetAppliedMigrationsAsync();
    }
}