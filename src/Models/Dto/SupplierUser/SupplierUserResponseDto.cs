namespace Jaxofy.Models.Dto.SupplierUser
{
    public class SupplierUserResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier's user account.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The user ID of the parent <see cref="ApplicationUser"/> containing the user's credentials, infos, etc...
        /// </summary>
        public long ApplicationUserId { get; set; }
    }
}