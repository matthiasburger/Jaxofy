using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// <see cref="Assignment"/>s may be rated by the involved <see cref="Client"/>s.
    /// </summary>
    [Table("Rating")]
    public class Rating : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies an <see cref="Employee"/>'s rating.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("Creation", Order = 1)]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModification", Order = 2)]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The <see cref="ClientUser"/> responsible for this <see cref="Rating"/>.
        /// </summary>
        [Column("ClientUser", Order = 3)]
        public ClientUser ClientUser { get; set; }

        /// <summary>
        /// The <see cref="Assignment"/> to which this <see cref="Rating"/> refers to.
        /// </summary>
        [Column("Assignment", Order = 4)]
        public Assignment Assignment { get; set; }

        /// <summary>
        /// The ID of the <see cref="Assignment"/> to which this <see cref="Rating"/> refers to.
        /// </summary>
        [Column("AssignmentId", Order = 5)]
        public long AssignmentId { get; set; }
    }
}