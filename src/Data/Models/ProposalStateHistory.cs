using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Enums;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// <see cref="Proposal"/> state record entry.
    /// <seealso cref="ProposalStateType"/>
    /// </summary>
    [Table("ProposalStateHistory")]
    public class ProposalStateHistory : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a dossier.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="ProposalStateType"/>.
        /// </summary>
        [Column("StateType", Order = 1)]
        public ProposalStateType StateType { get; set; }

        /// <summary>
        /// The ID of the dossier state modification entry's author and timestamp.
        /// </summary>
        [Column("CreationId", Order = 2)]
        public long? CreationId { get; set; }
        
        /// <summary>
        /// The dossier state modification entry's author and timestamp.
        /// </summary>
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// The ID of the <see cref="Proposal"/> to which this <see cref="ProposalStateHistory"/> belongs.
        /// </summary>
        [Column("ProposalId", Order = 3)]
        public long? ProposalId { get; set; }

        /// <summary>
        /// The <see cref="Proposal"/> to which this <see cref="ProposalStateHistory"/> belongs.
        /// </summary>
        [ForeignKey("ProposalId")]
        public Proposal Proposal { get; set; }
    }
}