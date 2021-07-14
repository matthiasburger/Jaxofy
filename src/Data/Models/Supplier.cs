using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Supplier.
    /// </summary>
    [Table("Supplier")]
    public class Supplier : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 1)]
        public long? CreationId { get; set; }
        
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 2)]
        public long? LastModificationId { get; set; }
        
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The supplier branch's postal address.
        /// </summary>
        [Column("PostalAddressId", Order = 3)]
        public long? PostalAddressId { get; set; }
        [ForeignKey("PostalAddressId")]
        public PostalAddress PostalAddress { get; set; }

        /// <summary>
        /// Branch email address. 
        /// </summary>
        [Column("Email", Order = 4)]
        public string Email { get; set; }

        /// <summary>
        /// The branch's phone number.
        /// </summary>
        [Column("Phone", Order = 5)]
        public string Phone { get; set; }

        /// <summary>
        /// FAX
        /// </summary>
        [Column("Fax", Order = 6)]
        public string Fax { get; set; }
        
        /// <summary>
        /// The ID of the top-level parent <see cref="SupplierHeader"/>
        /// associated with this <see cref="Supplier"/>.
        /// </summary>
        [Column("HeaderId", Order = 7)]
        public long? HeaderId { get; set; }
        
        /// <summary>
        /// The top-level parent <see cref="SupplierHeader"/>
        /// associated with this <see cref="Supplier"/>.
        /// </summary>
        [ForeignKey("HeaderId")]
        public SupplierHeader Header { get; set; }
        
        public ICollection<SupplierUserSetting> SupplierUserSettings { get; set; } = new List<SupplierUserSetting>();

        public ICollection<PoolEmployee> PoolEmployees { get; set; } = new List<PoolEmployee>();
    }
}