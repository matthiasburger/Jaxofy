using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IronSphere.Extensions;
using Jaxofy.Data;
using Jaxofy.Data.Models;
using Jaxofy.Extensions;
using Jaxofy.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Jaxofy.Services.AuthTokenService
{
    /// <summary>
    /// Auth token emission service. Use this to generate JWTs for your users!
    /// </summary>
    public class AuthTokenService : IAuthTokenService
    {
        private readonly ApplicationDbContext _db;
        private readonly IOptionsMonitor<JwtSettings> _jwtSettings;

        public AuthTokenService(ApplicationDbContext db, IOptionsMonitor<JwtSettings> jwtSettings)
        {
            _db = db;
            _jwtSettings = jwtSettings;
        }

        public async Task<string> EmitAuthTokenForUser(long userId)
        {
            string email = await _db.ApplicationUsers
                .Where(user => user.Id == userId && user.IsActive)
                .Select(user => user.Email)
                .FirstOrDefaultAsync();

            // TODO: ^^^^^ above: select the claims too (join, if necessary, depending on how Claims will be implemented).

            if (email.IsNullOrEmpty())
            {
                return null;
            }

            JwtSettings jwtSettings = _jwtSettings.CurrentValue;

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(jwtSettings.RSAPrivateKeyPEM.ToASN1bytesFromPEM(), out _);

            DateTime utcNow = DateTime.UtcNow;

            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                IssuedAt = utcNow,
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                NotBefore = utcNow.Subtract(TimeSpan.FromMinutes(4)),
                Expires = utcNow.AddMinutes(jwtSettings.LifetimeMinutes),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new("sub", userId.ToString()),
                    new("iss", jwtSettings.Issuer)

                    // TODO: add other user permission claims here too as soon as they are available
                }),
                SigningCredentials = new SigningCredentials
                (
                    algorithm: SecurityAlgorithms.RsaSsaPssSha512,
                    key: new RsaSecurityKey(rsa.ExportParameters(true))
                )
            };

            JsonWebTokenHandler jwtSecurityTokenHandler = new();
            return jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        }

        private string ExtractJwt(HttpContext httpContext)
        {
            string jwt = httpContext?.Request?.Headers?["Authorization"];

            if (jwt.IsNullOrEmpty())
            {
                return null;
            }

            return jwt.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase) ? jwt[7..] : null;
        }

        private long? _extractUserId(HttpContext httpContext)
        {
            try
            {
                JwtSecurityToken token = new JwtSecurityTokenHandler().ReadToken(ExtractJwt(httpContext)) as JwtSecurityToken;

                if (long.TryParse(token?.Subject, out long userId))
                    return userId;
            }
            catch
            {
                return null;
            }

            return null;
        }
        
        public long? ExtractUserId(HttpContext httpContext) => _extractUserId(httpContext);

        public async Task<ApplicationUser> GetApplicationUserAsync(HttpContext httpContext)
        {
            long? userId = _extractUserId(httpContext);
            if (!userId.HasValue)
                return null;

            return await _db.ApplicationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
        }
    }
}