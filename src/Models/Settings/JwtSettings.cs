namespace DasTeamRevolution.Models.Settings
{
    /// <summary>
    /// Settings class for authentication token validation and emission.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// The name of the settings section inside the appsettings.json file.
        /// </summary>
        public static string SectionName => "JwtSettings";
        
        /// <summary>
        /// Tokens should expire after this amount of minutes.
        /// </summary>
        public int LifetimeMinutes { get; set; }
        
        /// <summary>
        /// Private RSA Key (PEM-formatted PKCS#1 string) used by the server for signing authentication tokens.
        /// </summary>
        public string RSAPrivateKeyPEM { get; set; }
        
        /// <summary>
        /// Public RSA Key (PEM-formatted PKCS#8 string) usable by externals who wish to verify that the authentication tokens are signed using <see cref="RSAPrivateKeyPEM"/>.
        /// </summary>
        public string RSAPublicKeyPEM { get; set; }
        
        /// <summary>
        /// Should the "iss" claim be validated against the value of <see cref="Issuer"/>?
        /// </summary>
        public bool ValidateIssuer { get; set; }
        
        /// <summary>
        /// The value to set to every authentication token's "iss" claim before signing and emitting it. 
        /// </summary>
        public string Issuer { get; set; }
        
        /// <summary>
        /// Should the "aud" claim be validated against the value of <see cref="Audience"/>?
        /// </summary>
        public bool ValidateAudience { get; set; }
        
        /// <summary>
        /// The value to set to every authentication token's "aud" claim before signing and emitting it. 
        /// </summary>
        public string Audience { get; set; }
    }
}