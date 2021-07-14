using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Models.Dto.Auth
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
        /// Contains the <see cref="ClientHeader.Name"/> of the <see cref="ClientHeader"/> associated with the currently logged in <see cref="ClientUser"/> (if applicable).<para> </para>
        /// For <see cref="SupplierUser"/>s, the <see cref="SupplierHeader"/>'s <see cref="SupplierHeader.Name"/> is assigned here.
        /// </summary>
        public string HeaderName { get; set; } = string.Empty;

        /// <summary>
        /// Whether or not the user is required to change the user account password.
        /// </summary>
        public bool NewPasswordRequired { get; set; }

        public long? ClientUserId { get; set; }
        public long? SupplierUserId { get; set; }

        public long[] ClientIds { get; set; }
        public long[] SupplierIds { get; set; }
        public long ApplicationUserId { get; set; }
    }
}