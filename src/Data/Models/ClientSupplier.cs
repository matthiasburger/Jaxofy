using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Junction table for the client-supplier n:m relation.
    /// </summary>
    [Table("ClientSupplier")]
    public class ClientSupplier : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer/supplier association (n:m).
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// <see cref="Client"/> association ID.
        /// </summary>
        [Column("ClientId", Order = 1)]
        public long ClientId { get; set; }

        /// <summary>
        /// <see cref="Client"/> association.
        /// </summary>
        [Column("Client", Order = 2)]
        public Client Client { get; set; }
        
        /// <summary>
        /// <see cref="Supplier"/> association ID.
        /// </summary>
        [Column("SupplierId", Order = 3)]
        public long SupplierId { get; set; }

        /// <summary>
        /// <see cref="Supplier"/> association.
        /// </summary>
        [Column("Supplier", Order = 4)]
        public Supplier Supplier { get; set; }
        
        public ICollection<JobProfile> JobProfiles { get; set; }
    }
}