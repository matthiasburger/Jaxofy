using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jaxofy.Data.Models;
using Jaxofy.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace Jaxofy.Data
{
    public interface IDataSeeder
    {
        Task SeedData();
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

        public async Task SeedData()
        {
            if (_db.ApplicationUsers.Any())
               return;

            await _db.ApplicationUsers.AddAsync(new ()
            {
                Email = "matthias@fam-burger.de",
                Password = await _passwordHashing.HashPassword("matthias"),
                Username = "matthias",
                IsAdmin = true
            });

            await _db.SaveChangesAsync();
        }
    }
}