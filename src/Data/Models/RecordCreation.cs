using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A DB record that contains information about a DB entry's creation.
    /// </summary>
    [Table("RecordCreation")]
    public class RecordCreation : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a record's creation.
        /// </summary>
        [JsonIgnore]
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The ID of the <see cref="ApplicationUser"/> who created the database record.
        /// </summary>
        [Column("CreatedById", Order = 1)]
        public long? CreatedById { get; set; }

        /// <summary>
        /// The <see cref="ApplicationUser"/> who created the database record.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>
        /// Timestamp of the database data entry's creation.
        /// </summary>
        [Column("CreatedOn", Order = 2)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Constructs a new <see cref="RecordCreation"/> instance using a given <see cref="ApplicationUser"/><c>.</c><see cref="ApplicationUser.Id"/>.
        /// </summary>
        /// <param name="userId"><see cref="ApplicationUser"/><c>.</c><see cref="ApplicationUser.Id"/></param>
        public RecordCreation(long userId)
        {
            CreatedById = userId;
            CreatedOn = DateTime.Now;
        }

        /// <summary>
        /// Constructs a new <see cref="RecordCreation"/> instance without an author <see cref="ApplicationUser"/><c>.</c><see cref="ApplicationUser.Id"/>
        /// </summary>
        public RecordCreation()
        {
            CreatedOn = DateTime.Now;
        }
    }
}