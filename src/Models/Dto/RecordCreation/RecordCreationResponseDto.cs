using System;

namespace DasTeamRevolution.Models.Dto.RecordCreation
{
    public class RecordCreationResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a record's creation.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The ID of the <see cref="ApplicationUser"/> who created the database record.
        /// </summary>
        public long? CreatedById { get; set; }

        /// <summary>
        /// Timestamp of the database data entry's creation.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}