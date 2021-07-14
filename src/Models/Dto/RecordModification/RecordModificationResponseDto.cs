using System;

namespace DasTeamRevolution.Models.Dto.RecordModification
{
    public class RecordModificationResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a record's creation.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The ID of the <see cref="ApplicationUser"/> who modified the db record in question.
        /// </summary>
        public long? ModifiedById { get; set; }

        /// <summary>
        /// Timestamp of the modification.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}