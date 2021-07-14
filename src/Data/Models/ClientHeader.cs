using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A client header for easy data inception.
    /// </summary>
    [Table("ClientHeader")]
    public class ClientHeader : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer header.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="ClientGroup"/>s associated with this <see cref="ClientHeader"/>
        /// </summary>
        [Column("Groups", Order = 1)]
        public ICollection<ClientGroup> Groups { get; set; }
        
        /// <summary>
        /// Gets or sets the client's header name.
        /// </summary>
        [Column("Name", Order = 2)]
        public string Name { get; set; }
    }
}