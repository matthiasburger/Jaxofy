using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Models.Dto.Login
{
    /// <summary>
    /// Request DTO for the user login endpoint.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// <see cref="ApplicationUser.Email"/>
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// <see cref="ApplicationUser.Password"/>
        /// </summary>
        public string Password { get; set; }
    }
}