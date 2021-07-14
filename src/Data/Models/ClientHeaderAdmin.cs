using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Junction table for the <see cref="ClientHeader"/> - <see cref="ApplicationUser"/> N:M relation.
    /// </summary>
    [Table("ClientHeaderAdmin")]
    public class ClientHeaderAdmin : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a <see cref="ClientHeader"/> Admin association (n:m).
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// <see cref="ClientHeader"/> association ID.
        /// </summary>
        [Column("ClientHeaderId", Order = 1)]
        public long ClientHeaderId { get; set; }

        /// <summary>
        /// <see cref="ClientHeader"/> association.
        /// </summary>
        [Column("ClientHeader", Order = 2)]
        public ClientHeader ClientHeader { get; set; }

        /// <summary>
        /// Admin <see cref="ApplicationUser"/> association ID.
        /// </summary>
        [Column("AdminId", Order = 3)]
        public long AdminId { get; set; }

        /// <summary>
        /// Admin <see cref="ApplicationUser"/> association.
        /// </summary>
        [Column("Admin", Order = 4)]
        public ApplicationUser Admin { get; set; }
    }
}