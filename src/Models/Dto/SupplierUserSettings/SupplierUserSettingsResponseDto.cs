namespace Jaxofy.Models.Dto.SupplierUserSettings
{
    public class SupplierUserSettingsResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier/supplier user association (n:m).
        /// </summary>
        public long Id { get; set; }

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