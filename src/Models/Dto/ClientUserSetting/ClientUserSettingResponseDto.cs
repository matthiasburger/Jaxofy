namespace DasTeamRevolution.Models.Dto.ClientUserSetting
{
    public class ClientUserSettingResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer/customer user association (n:m).
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// <see cref="Client"/> association ID.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// <see cref="ClientUser"/> association ID.
        /// </summary>
        public long ClientUserId { get; set; }
    }
}