namespace DasTeamRevolution.Models.Dto.SupplierUser
{
    public class SupplierUserRequestDto
    {
        /// <summary>
        /// The user ID of the parent <see cref="ApplicationUser"/> containing the user's credentials, infos, etc...
        /// </summary>
        public long ApplicationUserId { get; set; }
    }
}