using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using Newtonsoft.Json;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A <see cref="Proposal"/> attachment.
    /// </summary>
    [Table("ProposalDocument")]
    public class ProposalDocument : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a <see cref="ProposalDocument"/>.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// ID of the <see cref="Proposal"/> to which this attachment belongs.
        /// </summary>
        [Column("ProposalId", Order = 1)]
        public long? ProposalId { get; set; }

        /// <summary>
        /// <see cref="Proposal"/> to which this <see cref="ProposalDocument"/> belongs.
        /// </summary>
        [ForeignKey("ProposalId"), JsonIgnore]
        public Proposal Proposal { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("Creation", Order = 2)]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModification", Order = 3)]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The document's content bytes.
        /// </summary>
        [Column("DocumentBytes", Order = 4)]
        public byte[] DocumentBytes { get; set; }

        /// <summary>
        /// Document name (typically, this will be the underlying filename).
        /// </summary>
        [Column("DocumentName", Order = 5)]
        public string DocumentName { get; set; }
    }
}