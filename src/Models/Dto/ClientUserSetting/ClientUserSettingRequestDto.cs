namespace Jaxofy.Models.Dto.ClientUserSetting
{
    public class ClientUserSettingRequestDto
    {
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