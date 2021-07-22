using System.Threading.Tasks;
using Jaxofy.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Jaxofy.Services.AuthTokenService
{
    /// <summary>
    /// Auth token service interface for emitting tokens, querying against them, etc...
    /// </summary>
    public interface IAuthTokenService
    {
        /// <summary>
        /// Emits a new authentication token for a given <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user's <see cref="ApplicationUser.Id"/> for whom to generate the token.</param>
        /// <returns>The token <see cref="string"/> ready for the user to append to his subsequent requests (inside the <c>Authorization</c> HTTP header with the <c>Bearer </c> prefix). <c>null</c> if the specified <paramref name="userId"/> does not exist in the db or is inactive.</returns>
        Task<string> EmitAuthTokenForUser(long userId);

        /// <summary>
        /// Extracts the <see cref="ApplicationUser.Id"/> from the <see cref="ApplicationUser"/>'s auth token.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <returns>The found user ID; <c>null</c> in case of a failure.</returns>
        public long? ExtractUserId(HttpContext httpContext);

        Task<ApplicationUser> GetApplicationUserAsync(HttpContext httpContext);
    }
}