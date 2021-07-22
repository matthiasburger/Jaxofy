using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Jaxofy.Controllers.Base;
using Jaxofy.Data.Models;
using Jaxofy.Data.Repositories;
using Jaxofy.Models.Dto.ApplicationUser;
using Jaxofy.Services.PasswordHashing;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jaxofy.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ApplicationUsersController : ApiBaseController
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IPasswordHashing _passwordHashing;
        private readonly IMapper _mapper;

        public ApplicationUsersController(IApplicationUserRepository applicationUserRepository,
            IPasswordHashing passwordHashing, IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository;
            _passwordHashing = passwordHashing;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(ODataQueryOptions<ApplicationUser> query)
        {
            IQueryable<ApplicationUser> applicationUsersQueryable =
                _applicationUserRepository.GetQueryable().AsNoTracking();

            IQueryable applicationUsers = query.ApplyTo(applicationUsersQueryable);

            IEnumerable<ApplicationUserResponseDto> userResponse =
                _mapper.Map<List<ApplicationUserResponseDto>>(applicationUsers);

            return EnvelopeResult.Ok(userResponse);
        }

        [HttpGet]
        [Route("{userId:long}")]
        public async Task<IActionResult> Get(long userId)
        {
            ApplicationUser user = await _applicationUserRepository
                .SingleOrDefaultNoTracking(x => x.Id == userId);

            if (user is null)
                return Error(404, Constants.Errors.UserNotFound);

            ApplicationUserResponseDto userResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);

            return EnvelopeResult.Ok(userResponseDto);
        }

        // todo: change this to register
        [HttpPost, Route("create-dev"), AllowAnonymous]
        public async Task<IActionResult> CreateUser(string email, string password, string firstName, string lastName)
        {
            if (await _applicationUserRepository.AnyNoTracking(x => x.Username == email))
                return Error(400, Constants.Errors.UserAlreadyExists);

            ApplicationUser user = new()
            {
                Email = email,
                Username = email,
                Password = await _passwordHashing.HashPassword(password),
                CreatedOn = DateTime.UtcNow,
                IsAdmin = true,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true
            };

            try
            {
                (bool success, EntityEntry<ApplicationUser> entity) = await _applicationUserRepository.Add(user);
                if (!success)
                    return Error(400, Constants.Errors.UserCreationFailed);

                ApplicationUserResponseDto createdUserDto = _mapper.Map<ApplicationUserResponseDto>(entity.Entity);

                return EnvelopeResult.Created(
                    Url.Action("Get", "ApplicationUsers", new {userId = entity.Entity.Id}),
                    createdUserDto);
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}