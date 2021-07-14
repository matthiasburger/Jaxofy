using System.ComponentModel.DataAnnotations.Schema;
using Jaxofy.Models.Dto.RecordCreation;
using Jaxofy.Models.Dto.TimeRecord;
using Jaxofy.Models.Enums;

namespace Jaxofy.Models.Dto.TimeRecordStateHistory
{
    public class TimeRecordStateHistoryResponseDto
    {
        /// <summary>
        /// Uniquely identifying number (<see cref="TimeRecordStateHistory"/> primary key).
        /// </summary>
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
        public RecordCreationResponseDto Creation { get; set; }

        /// <summary>
        /// The ID of the <see cref="TimeRecord"/> to which this state record belongs.
        /// </summary>
        [Column("TimeRecordId", Order = 3)]
        public long? TimeRecordId { get; set; }

        /// <summary>
        /// The <see cref="TimeRecord"/> to which this state record belongs.
        /// </summary>
        [ForeignKey("TimeRecordId")]
        public TimeRecordResponseDto TimeRecord { get; set; }
    }
}