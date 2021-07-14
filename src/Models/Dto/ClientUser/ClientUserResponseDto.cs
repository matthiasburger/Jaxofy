namespace DasTeamRevolution.Models.Dto.ClientUser
{
    public class ClientUserResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer's user account.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// The parent <see cref="ApplicationUser"/>'s ID.
        /// </summary>
        public long ApplicationUserId { get; set; }
    }
}