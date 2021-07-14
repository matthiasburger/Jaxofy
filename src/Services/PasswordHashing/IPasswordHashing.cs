using System.Threading.Tasks;

namespace Jaxofy.Services.PasswordHashing
{
    /// <summary>
    /// Service interface for hashing passwords.
    /// </summary>
    public interface IPasswordHashing
    {
        /// <summary>
        /// Hashes a given <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The pw to hash.</param>
        /// <returns>The hashed password, ready to be stored in the db safely.</returns>
        Task<string> HashPassword(string password);

        /// <summary>
        /// Verifies a given <paramref name="password"/> against a known password's <paramref name="hash"/>.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The hash to verify the pw against.</param>
        /// <returns><c>true</c> if the password is correct; <c>false</c> if otherwise.</returns>
        Task<bool> Verify(string password, string hash);
    }
}