using Microsoft.Extensions.Configuration;

namespace DasTeamRevolution.Services.Configuration
{
    /// <summary>
    /// Service for generating an <see cref="IConfiguration"/> instance (freshly allocated! files will be read from disk upon construction).
    /// that contains all the configs for the platform you're currently on (e.g. CLI args, Env variables, secrets, etc...).
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Get an <see cref="IConfiguration"/> with all the necessary settings
        /// and key-value pairs for the platform you're currently on.
        /// </summary>
        /// <param name="cliArgs">[OPTIONAL] CLI arguments to also mix into the <see cref="IConfiguration"/> instance.</param>
        /// <returns><see cref="IConfiguration"/></returns>
        IConfiguration GetPlatformAgnosticConfig(string[] cliArgs = null);
    }
}