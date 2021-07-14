namespace Jaxofy.Models.Dto.SupplierUserSettings
{
    public class SupplierUserSettingsRequestDto
    {
        /// <summary>
        /// <see cref="Supplier"/> association ID.
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// <see cref="SupplierUser"/> association ID.
        /// </summary>
        public long SupplierUserId { get; set; }
    }
}