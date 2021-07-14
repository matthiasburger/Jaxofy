using DasTeamRevolution.Models.Dto.PostalAddress;

namespace DasTeamRevolution.Models.Dto.Supplier
{
    public class SupplierRequestDto
    {
        /// <summary>
        /// The supplier branch's postal address.
        /// </summary>
        public PostalAddressRequestDto PostalAddress { get; set; }

        /// <summary>
        /// Branch email address. 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The branch's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// FAX
        /// </summary>
        public string Fax { get; set; }
        
        /// <summary>
        /// The ID of the top-level parent <see cref="SupplierHeader"/>
        /// associated with this <see cref="Supplier"/>.
        /// </summary>
        public long? HeaderId { get; set; }

    }
}