using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A supplier's user account.
    /// </summary>
    [Table("SupplierUser")]
    public class SupplierUser : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a supplier's user account.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The user ID of the parent <see cref="ApplicationUser"/> containing the user's credentials, infos, etc...
        /// </summary>
        [Column("ApplicationUserId", Order = 1)]
        public long ApplicationUserId { get; set; }

        /// <summary>
        /// The parent <see cref="ApplicationUser"/> containing the user's credentials, infos, etc...
        /// </summary>
        [Column("ApplicationUser", Order = 2)]
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// The <see cref="Proposal"/>s associated with this <see cref="SupplierUser"/> account.
        /// </summary>
        [Column("Proposals", Order = 3)]
        public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

        public ICollection<SupplierUserSetting> SupplierUserSettings { get; set; } = new List<SupplierUserSetting>();
    }
}