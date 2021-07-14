using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Enums;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// <see cref="TimeRecord"/> state record entry.
    /// </summary>
    [Table("TimeRecordStateHistory")]
    public class TimeRecordStateHistory : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a time table entry's state record.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="TimeRecordStateType"/>.
        /// </summary>
        [Column("StateType", Order = 1)]
        public TimeRecordStateType StateType { get; set; }

        /// <summary>
        /// The time record state modification entry's author and timestamp.
        /// </summary>
        [Column("Creation", Order = 2)]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// The ID of the <see cref="TimeRecord"/> to which this state record belongs.
        /// </summary>
        [Column("TimeRecordId", Order = 3)]
        public long? TimeRecordId { get; set; }

        /// <summary>
        /// The <see cref="TimeRecord"/> to which this state record belongs.
        /// </summary>
        [ForeignKey("TimeRecordId")]
        public TimeRecord TimeRecord { get; set; }
    }
}