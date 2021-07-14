using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ApplicationUser;
using DasTeamRevolution.Models.Filters;
using DasTeamRevolution.Services.Environment;
using DasTeamRevolution.Services.PasswordHashing;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DasTeamRevolution.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ApplicationUsersController : ApiBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHashing _passwordHashing;
        private readonly IEnvironmentDiscovery _environmentDiscovery;
        private readonly IMapper _mapper;

        public ApplicationUsersController(ApplicationDbContext db, IEnvironmentDiscovery environmentDiscovery,
            IPasswordHashing passwordHashing, IMapper mapper)
        {
            _db = db;
            _passwordHashing = passwordHashing;
            _environmentDiscovery = environmentDiscovery;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
        {
            paginationFilter.Clamp();

            IOrderedQueryable<ApplicationUser> query = _db.ApplicationUsers
                .AsNoTracking()
                .OrderByDescending(user => user.CreatedOn)
                .ThenBy(user => user.LastName);

            long totalItemCount = await query.LongCountAsync();

            List<ApplicationUser> users = await query
                .Skip(paginationFilter.PageSize * (paginationFilter.Page - 1))
                .Take(paginationFilter.PageSize)
                .ToListAsync();

            List<ApplicationUserResponseDto> userResponseDto = _mapper.Map<List<ApplicationUserResponseDto>>(users);

            return Ok(totalItemCount, userResponseDto);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(long userId)
        {
            ApplicationUser user = await _db.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return Error(404, Constants.Errors.UserNotFound);
            }

            ApplicationUserResponseDto userResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);

            return Ok(1, new[] {userResponseDto});
        }

        [HttpPost, Route("create-dev"), AllowAnonymous]
        public async Task<IActionResult> CreateUser(string email, string password, string firstName, string lastName)
        {
            /*if (!_environmentDiscovery.IsTestEnvironment)
            {
                return Forbidden();
            }*/

            if (await _db.ApplicationUsers.AsNoTracking().AnyAsync(u => u.Email == email))
            {
                return Error(400, Constants.Errors.UserAlreadyExists);
            }

            ApplicationUser user = new()
            {
                Email = email,
                Password = await _passwordHashing.HashPassword(password),
                CreatedOn = DateTime.UtcNow,
                IsAdmin = true,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true
            };

            try
            {
                EntityEntry<ApplicationUser> addedUser = await _db.AddAsync(user);
                await _db.SaveChangesAsync();

                ApplicationUserResponseDto createdUserDto = _mapper.Map<ApplicationUserResponseDto>(addedUser.Entity);

                return Created($"/api/v1/applicationuser/{addedUser.Entity.Id}", 1, new[] {createdUserDto});
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}