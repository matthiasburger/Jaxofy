using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A customer's user account.
    /// </summary>
    [Table("ClientUser")]
    public class ClientUser : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a customer's user account.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The parent <see cref="ApplicationUser"/>'s ID.
        /// </summary>
        [Column("ApplicationUserId", Order = 1)]
        public long ApplicationUserId { get; set; }
        
        /// <summary>
        /// The parent <see cref="ApplicationUser"/> containing the user's credentials, infos, etc...
        /// </summary>
        [Column("ApplicationUser", Order = 2)]
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// The <see cref="Rating"/>s authored by this user.
        /// </summary>
        [Column("Ratings", Order = 3)]
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        /// <summary>
        /// The <see cref="TimeRecord"/>s for which this user is responsible.
        /// </summary>
        [Column("TimeRecords", Order = 4)]
        public ICollection<TimeRecord> TimeRecords { get; set; } = new List<TimeRecord>();
        
        public ICollection<ClientUserSetting> ClientUserSettings { get; set; } = new List<ClientUserSetting>();
    }
}