namespace DasTeamRevolution.Models.Dto.Auth
{
    /// <summary>
    /// Response body DTO for the public key GET endpoint.
    /// </summary>
    public class SigningPublicKeyResponseDto
    {
        /// <summary>
        /// The public key of the server's JWT signing keypair
        /// (clients may require this and verify if an auth token truly was emitted by the acclaimed issuer).
        /// </summary>
        public string PublicKey { get; set; }
    }
}