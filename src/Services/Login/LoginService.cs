using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Jaxofy.Data;
using Jaxofy.Models.Settings;
using Jaxofy.Services.AuthTokenService;
using Jaxofy.Services.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jaxofy.Services.Login
{
    /// <summary>
    /// Login service provider.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHashing _pwHashing;
        private readonly IAuthTokenService _authTokenService;
        private readonly IOptionsMonitor<JwtSettings> _jwtSettings;

        public LoginService(ApplicationDbContext db, IOptionsMonitor<JwtSettings> jwtSettings, IPasswordHashing pwHashing, IAuthTokenService authTokenService)
        {
            _db = db;
            _pwHashing = pwHashing;
            _jwtSettings = jwtSettings;
            _authTokenService = authTokenService;
        }

        public async Task<string> Login(string email, string password)
        {
            (long userId, string storedPassword) = await _db.ApplicationUsers
                .Where(user => user.Email == email && user.IsActive)
                .Select(user => new ValueTuple<long, string>(user.Id, user.Password))
                .FirstOrDefaultAsync();

            if (storedPassword.IsNullOrEmpty())
            {
                return null;
            }

            if (!await _pwHashing.Verify(password, storedPassword))
            {
                // TODO: handle login failures. E.g. log them, increase failed attempts count, etc...
                
                return null;
            }

            return await _authTokenService.EmitAuthTokenForUser(userId);
        }

        public Task<string> RefreshToken(string token)
        {
            var jwtSettings = _jwtSettings.CurrentValue;

            using var rsa = RSA.Create();
            rsa.ImportFromPem(jwtSettings.RSAPublicKeyPEM);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa.ExportParameters(false)),
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                NameClaimType = ClaimTypes.NameIdentifier,
                ClockSkew = TimeSpan.FromSeconds(4)
            };

            var jwtHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal result = jwtHandler.ValidateToken(token, tokenValidationParameters, out _);

                if (result?.Identity is null || !long.TryParse(result.Identity.Name, out long userId))
                {
                    return null;
                }

                return _authTokenService.EmitAuthTokenForUser(userId);
            }
            catch
            {
                return null;
            }
        }
    }
}