using System;
using System.Linq;
using System.Threading.Tasks;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Auth;
using DasTeamRevolution.Models.Dto.Login;
using DasTeamRevolution.Models.Settings;
using DasTeamRevolution.Services.AuthTokenService;
using DasTeamRevolution.Services.Login;
using IronSphere.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DasTeamRevolution.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly ILoginService _loginService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthTokenService _authTokenService;
        private readonly IOptionsMonitor<JwtSettings> _jwtSettingsMonitor;

        public AuthController(ILoginService loginService, IOptionsMonitor<JwtSettings> jwtSettingsMonitor, IAuthTokenService authTokenService, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _loginService = loginService;
            _authTokenService = authTokenService;
            _jwtSettingsMonitor = jwtSettingsMonitor;
        }

        [AllowAnonymous]
        [HttpGet, Route("public-key")]
        public IActionResult GetSigningPublicKey()
        {
            return Ok(1, new[] { new SigningPublicKeyResponseDto { PublicKey = _jwtSettingsMonitor.CurrentValue.RSAPublicKeyPEM } });
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (dto.Email.IsNullOrEmpty() || dto.Password.IsNullOrEmpty())
                return Error(401, Constants.Errors.LoginFailed);

            string token = await _loginService.Login(dto.Email, dto.Password);

            if (token.IsNullOrEmpty())
                return Error(401, Constants.Errors.LoginFailed);

            return Ok(1, new[] { new LoginResponseDto { Token = token } });
        }

        [HttpGet, Authorize, Route("login/extend")]
        public async Task<IActionResult> ExtendToken()
        {
            string jwt = HttpContext?.Request.Headers[HeaderNames.Authorization];

            if (jwt.IsNullOrEmpty())
                return Error(403, Constants.Errors.LoginFailed);

            string token = await _loginService.RefreshToken(jwt.Replace("Bearer ", string.Empty));

            if (token.IsNullOrEmpty())
                return Error(403, Constants.Errors.LoginFailed);

            return Ok(1, new[] { new LoginResponseDto { Token = token } });
        }

        [HttpGet, Authorize, Route("whoami")]
        public async Task<IActionResult> GetUserMetadata()
        {
            string jwt = HttpContext?.Request.Headers[HeaderNames.Authorization];

            if (jwt.IsNullOrEmpty())
                return Error(403, Constants.Errors.LoginFailed);

            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
                return Error(403, Constants.Errors.LoginFailed);

            ApplicationUser user = await _dbContext
                .ApplicationUsers
                .Where(u => u.Id == userId && u.IsActive)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return Error(403, Constants.Errors.LoginFailed);
            }

            UserMetadataResponseDto response = new()
            {
                UserType = nameof(ApplicationUser),
                NewPasswordRequired = user.NewPasswordRequiredOn.HasValue && user.NewPasswordRequiredOn <= DateTime.UtcNow,
                ApplicationUserId = user.Id
            };

            ClientUser clientUser = await _dbContext
                .ClientUsers
                .Where(u => u.ApplicationUserId == user.Id)
                .FirstOrDefaultAsync();

            if (clientUser is not null)
            {
                response.UserType = nameof(ClientUser);
                response.ClientUserId = clientUser.Id;

                IQueryable<Client> clientQuery =
                    from clientUserSetting in _dbContext.ClientUserPermissions.Where(p =>
                        p.ClientUserId == clientUser.Id)
                    from client in _dbContext.Clients.Where(c => c.Id == clientUserSetting.ClientId)
                    select client;

                response.ClientIds = await clientQuery.Select(x => x.Id).ToArrayAsync();
                    
                IQueryable<string> clientHeaderNameQuery =
                    from client in clientQuery
                    from clientGroup in _dbContext.ClientGroups.Where(g => g.Id == client.GroupId)
                    from clientHeader in _dbContext.ClientHeaders.Where(h => h.Id == clientGroup.HeaderId)
                    select clientHeader.Name;

                response.HeaderName = await clientHeaderNameQuery.FirstOrDefaultAsync() ?? string.Empty;

                return Ok(1, new[] { response });
            }

            SupplierUser supplierUser = await _dbContext
                .SupplierUsers
                .Where(u => u.ApplicationUserId == user.Id)
                .FirstOrDefaultAsync();

            if (supplierUser is not null)
            {
                response.UserType = nameof(SupplierUser);
                response.SupplierUserId = supplierUser.Id;

                IQueryable<Supplier> supplierQuery =
                    from supplierUserSetting in _dbContext.SupplierUserPermissions.Where(p =>
                        p.SupplierUserId == supplierUser.Id)
                    from supplier in _dbContext.Suppliers.Where(s => s.Id == supplierUserSetting.SupplierId)
                    select supplier;
                response.SupplierIds = await supplierQuery.Select(x=>x.Id).ToArrayAsync();

                IQueryable<string> supplierHeaderNameQuery =
                    from supplier in supplierQuery
                    from supplierHeader in _dbContext.SupplierHeaders.Where(h => h.Id == supplier.HeaderId)
                    select supplierHeader.Name;

                response.HeaderName = await supplierHeaderNameQuery.FirstOrDefaultAsync() ?? string.Empty;

                return Ok(1, new[] { response });
            }

            return Ok(1, new[] { response });
        }
    }
}