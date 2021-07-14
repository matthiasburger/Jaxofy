using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Junction table for the <see cref="Supplier"/>/<see cref="SupplierUser"/>
    /// permissions relationship (n:m).
    /// </summary>
    [Table("SupplierUserSetting")]
    public class SupplierUserSetting : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier/supplier user association (n:m).
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// <see cref="Supplier"/> association ID.
        /// </summary>
        [Column("SupplierId", Order = 1)]
        public long SupplierId { get; set; }

        /// <summary>
        /// <see cref="Supplier"/> association.
        /// </summary>
        [Column("Supplier", Order = 2)]
        public Supplier Supplier { get; set; }

        /// <summary>
        /// <see cref="SupplierUser"/> association ID.
        /// </summary>
        [Column("SupplierUserId", Order = 3)]
        public long SupplierUserId { get; set; }

        /// <summary>
        /// <see cref="SupplierUser"/> association.
        /// </summary>
        [Column("SupplierUser", Order = 4)]
        public SupplierUser SupplierUser { get; set; }
    }
}