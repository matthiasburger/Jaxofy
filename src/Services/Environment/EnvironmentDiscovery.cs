using System;

using IronSphere.Extensions;

namespace DasTeamRevolution.Services.Environment
{
    /// <inheritdoc cref="IEnvironmentDiscovery"/>
    public class EnvironmentDiscovery : IEnvironmentDiscovery
    {
        private static string _getVariable(string variable, EnvironmentVariableTarget target) 
            => System.Environment.GetEnvironmentVariable(variable, target);

        /// <inheritdoc cref="IEnvironmentDiscovery"/>
        public bool IsTestEnvironment
        {
            get
            {
                string environmentVariableDevelopment =
                    _getVariable("DasTeamRevolutionDevelopment", EnvironmentVariableTarget.User)
                    ?? _getVariable("DasTeamRevolutionDevelopment", EnvironmentVariableTarget.Process);

                if (environmentVariableDevelopment is null)
                    return false;

                return !environmentVariableDevelopment.IsNullOrWhiteSpace()
                       && (environmentVariableDevelopment == "1" ||
                           environmentVariableDevelopment.ToLowerInvariant() == "true");
            }
        }

        public bool IsDocker
        {
            get
            {
                string docker =
                    _getVariable("DasTeamRevolutionDocker", EnvironmentVariableTarget.User) 
                    ?? _getVariable("DasTeamRevolutionDocker", EnvironmentVariableTarget.Process);
                
                return !docker.IsNullOrWhiteSpace() && (docker == "1" || docker?.ToLowerInvariant() == "true");
            }
        }
    }
}