using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Junction table for the <see cref="Client"/>/<see cref="ClientUser"/>
    /// permissions (and may also contain various other settings in the future...) relationship (n:m).
    /// </summary>
    [Table("ClientUserSetting")]
    public class ClientUserSetting : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer/customer user association (n:m).
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
        /// <see cref="ClientUser"/> association ID.
        /// </summary>
        [Column("ClientUserId", Order = 3)]
        public long ClientUserId { get; set; }

        /// <summary>
        /// <see cref="ClientUser"/> association.
        /// </summary>
        [Column("ClientUser", Order = 4)]
        public ClientUser ClientUser { get; set; }
    }
}