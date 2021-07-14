namespace Jaxofy.Models.Dto.Login
{
    /// <summary>
    /// Response body DTO for successful login requests.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Freshly generated user auth token.
        /// </summary>
        public string Token { get; set; }
    }
}