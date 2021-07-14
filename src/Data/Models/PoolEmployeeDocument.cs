using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using Newtonsoft.Json;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Document uploaded by a <see cref="PoolEmployee"/>.
    /// </summary>
    public class PoolEmployeeDocument : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a <see cref="PoolEmployee"/>.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// <see cref="PoolEmployee"/> responsible for this document.
        /// </summary>
        [Column("PoolEmployeeId", Order = 1)]
        public long? PoolEmployeeId { get; set; }

        /// <summary>
        /// <see cref="PoolEmployee"/> responsible for this document.
        /// </summary>
        [ForeignKey("PoolEmployeeId"), JsonIgnore]
        public PoolEmployee PoolEmployee { get; set; }

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