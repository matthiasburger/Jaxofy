using System.IO;
using System.Linq;
using DasTeamRevolution.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data
{
    public interface IDataSeeder
    {
        void SeedData();
    }

    public class DataSeeder : IDataSeeder
    {
        private readonly IPasswordHashing _passwordHashing;
        private readonly ApplicationDbContext _db;

        public DataSeeder(IPasswordHashing passwordHashing, ApplicationDbContext db)
        {
            _passwordHashing = passwordHashing;
            _db = db;
        }

        public void SeedData()
        {
            if (_db.ApplicationUsers.Any())
            {
               return;
            }

            string sql = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data", "DemoSql", "demodata.sql"));

            _db.Database.ExecuteSqlRaw(sql);
            
        }
    }
}