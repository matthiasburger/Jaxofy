using System.IO;
using System.Linq;
using Jaxofy.Data.Models;
using Jaxofy.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace Jaxofy.Data
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
               return;

            _db.ApplicationUsers.Add(new ApplicationUser
            {
                Email = "matthias@fam-burger.de",
                Password = "matthias",
                Username = "matthias",
                IsAdmin = true
            });

            _db.SaveChanges();
        }
    }
}