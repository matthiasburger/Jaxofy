using System.Threading.Tasks;
using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Services.Login
{
    /// <summary>
    /// Service interface for user login endpoint interactions.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Attempts login for a given credential tuple of <paramref name="email"/> + <paramref name="password"/>.
        /// </summary>
        /// <param name="email"><see cref="ApplicationUser.Email"/></param>
        /// <param name="password">The user's password (will be hashed and compared to what is stored in the db as <see cref="ApplicationUser.Password"/>).</param>
        /// <returns>If login fails, <c>null</c> is returned; on success: a fresh auth token for the user to append to his subsequent requests (into the "Authorization" HTTP-Header with the "Bearer " prefix).</returns>
        Task<string> Login(string email, string password);
        
        /// <summary>
        /// Attempts refreshing a user auth <paramref name="token"/>.
        /// Useful for extending the lifetime of existing tokens that are about to expire.
        /// </summary>
        /// <param name="token">The token that is about to expire that deserves some freshness.</param>
        /// <returns>Fresh token, very valid, 100% BIO</returns>
        Task<string> RefreshToken(string token);
    }
}