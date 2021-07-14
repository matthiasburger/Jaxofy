using Jaxofy.Models.Dto.RecordCreation;
using Jaxofy.Models.Dto.RecordModification;

namespace Jaxofy.Models.Dto.SupplierHeader
{
    public class SupplierHeaderResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier header.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Name of the client header.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        public RecordCreationResponseDto Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        public RecordModificationResponseDto LastModification { get; set; }
    }
}