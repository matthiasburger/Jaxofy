using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A data entry that contains information about a specific database record's modification.
    /// </summary>
    [Table("RecordModification")]
    public class RecordModification : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a record's creation.
        /// </summary>
        [JsonIgnore]
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The ID of the <see cref="ApplicationUser"/> who modified the db record in question.
        /// </summary>
        [Column("ModifiedById", Order = 1)]
        public long? ModifiedById { get; set; }

        /// <summary>
        /// The <see cref="ApplicationUser"/> who modified the db record in question.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("ModifiedById")]
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>
        /// Timestamp of the modification.
        /// </summary>
        [Column("ModifiedOn", Order = 2)]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// The reason of the modification.
        /// </summary>
        [Column("Reason", Order = 3)]
        public string Reason { get; set; }
    }
}