using DasTeamRevolution.Models.Dto.PostalAddress;
using DasTeamRevolution.Models.Dto.RecordCreation;
using DasTeamRevolution.Models.Dto.RecordModification;

namespace DasTeamRevolution.Models.Dto.Supplier
{
    public class SupplierResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Entry creation metadata.
        /// </summary>
        public RecordCreationResponseDto Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        public RecordModificationResponseDto LastModification { get; set; }

        /// <summary>
        /// The supplier branch's postal address.
        /// </summary>
        public PostalAddressResponseDto PostalAddress { get; set; }

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