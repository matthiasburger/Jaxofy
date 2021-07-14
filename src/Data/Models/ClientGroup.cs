using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Logical union of one or more customer branches.
    /// </summary>
    [Table("ClientGroup")]
    public class ClientGroup : RecursiveEntity<ClientGroup, long>
    {
        /// <summary>
        /// The <see cref="Client"/>s that belong to this <see cref="ClientGroup"/>.
        /// </summary>
        [Column("Clients", Order = 1)]
        public ICollection<Client> Clients { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 2)]
        public long? CreationId { get; set; }
        
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 3)]
        public long? LastModificationId { get; set; }
        
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }
        
        /// <summary>
        /// The top-level parent <see cref="ClientHeader"/> associated with this <see cref="ClientGroup"/>.
        /// </summary>
        [Column("HeaderId", Order = 4)]
        public long? HeaderId { get; set; }
        
        [ForeignKey("HeaderId")]
        public ClientHeader Header { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the clients group
        /// </summary>
        [Column("Name", Order= 5)] 
        public string Name { get; set; }
    }
}