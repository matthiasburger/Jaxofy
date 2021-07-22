using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Jaxofy.Controllers.Base;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Jaxofy.Data.Repositories;
using Jaxofy.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jaxofy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DataAdministrationController : ApiBaseController
    {
        private readonly IMigrationRepository _migrationRepository;
        private readonly IDataSeeder _dataSeed;
        private readonly IAuthTokenService _authTokenService;
        
        public DataAdministrationController(IMigrationRepository migrationRepository, IDataSeeder dataSeed, IAuthTokenService authTokenService)
        {
            _migrationRepository = migrationRepository;
            _dataSeed = dataSeed;
            _authTokenService = authTokenService;
        }

        [HttpPost("drop-database")]
        public async Task<IActionResult> DropDatabase()
        {
            ApplicationUser applicationUser = 
                await _authTokenService.GetApplicationUserAsync(HttpContext);
            
            if (!applicationUser.IsAdmin)
                return Forbidden();
            
            await _migrationRepository.RemoveDatabase();
            return Ok();
        }

        [HttpPost("update-database"), AllowAnonymous]
        public async Task<IActionResult> ApplyMigration(string migration)
        {
            // only allow to downgrade database if you're an admin-user
            if (!migration.IsNullOrEmpty())
            {
                ApplicationUser applicationUser = 
                    await _authTokenService.GetApplicationUserAsync(HttpContext);
                
                if (!applicationUser.IsAdmin)
                    return Forbidden();
            }

            await _migrationRepository.ApplyMigration(migration);
            return Ok();
        }

        [HttpPost("seed"), AllowAnonymous]
        public IActionResult Seed()
        {
            _dataSeed.SeedData();
            return Ok();
        }

        [HttpGet("get-migrations")]
        public async Task<IActionResult> GetMigrations()
        {
            IEnumerable<(string migration, bool applied)> migrations =
                (await _migrationRepository.GetAppliedMigrations())
                .Select(x => (x, true))
                .Concat((await _migrationRepository.GetPendingMigrations()).Select(x => (x, false)));

            return Ok(migrations);
        }
    }
}