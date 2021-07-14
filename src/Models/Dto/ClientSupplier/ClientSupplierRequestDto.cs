namespace Jaxofy.Models.Dto.ClientSupplier
{
    public class ClientSupplierRequestDto
    {
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