using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Junction table for client/branch profile mappings
    /// (many-to-many: see db schema for more information).
    /// </summary>
    [Table("ClientEmployeeProfile")]
    public class ClientEmployeeProfile : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a profile.
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
        /// <see cref="EmployeeProfile"/> association ID.
        /// </summary>
        [Column("EmployeeProfileId", Order = 3)]
        public long EmployeeProfileId { get; set; }

        /// <summary>
        /// <see cref="EmployeeProfile"/> association.
        /// </summary>
        [Column("EmployeeProfile", Order = 4)]
        public EmployeeProfile EmployeeProfile { get; set; }
    }
}