using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A supplier header for easy data inception.
    /// </summary>
    [Table("SupplierHeader")]
    public class SupplierHeader : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier header.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="Supplier"/>s associated with this <see cref="SupplierHeader"/>
        /// </summary>
        [Column("Suppliers", Order = 1)]
        public ICollection<Supplier> Groups { get; set; }

        /// <summary>
        /// Name of the client header.
        /// </summary>
        [Column("Name", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 3)]
        public long? CreationId { get; set; }
        
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 4)]
        public long? LastModificationId { get; set; }
        
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }
    }
}