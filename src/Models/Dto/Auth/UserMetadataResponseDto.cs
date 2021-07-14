namespace Jaxofy.Models.Dto.Auth
{
    /// <summary>
    /// Response body DTO for user metadata fetching.
    /// </summary>
    public class UserMetadataResponseDto
    {
        /// <summary>
        /// Describes of which type a user is.
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Whether or not the user is required to change the user account password.
        /// </summary>
        public bool NewPasswordRequired { get; set; }
        public long ApplicationUserId { get; set; }
    }
}