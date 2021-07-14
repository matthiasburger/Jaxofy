using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// An <see cref="Employee"/>'s document.
    /// </summary>
    [Table("EmployeeDocument")]
    public class EmployeeDocument : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies an employee (aka "TMA").
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// <see cref="Employee"/> associated with this document.
        /// </summary>
        [Column("EmployeeId", Order = 1)]
        public long? EmployeeId { get; set; }

        /// <summary>
        /// <see cref="PoolEmployee"/> associated with this document.
        /// </summary>
        [ForeignKey("EmployeeId"), JsonIgnore]
        public Employee Employee { get; set; }

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