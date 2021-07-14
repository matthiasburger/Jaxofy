namespace DasTeamRevolution.Models.Dto.ClientSupplier
{
    public class ClientSupplierResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer/supplier association (n:m).
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// <see cref="Client"/> association ID.
        /// </summary>]
        public long ClientId { get; set; }
        
        /// <summary>
        /// <see cref="Supplier"/> association ID.
        /// </summary>
        public long SupplierId { get; set; }
    }
}