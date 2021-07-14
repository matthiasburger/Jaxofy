namespace Jaxofy.Models.Dto.TimeRecord
{
    public class TimeRecordRequestDto
    {
        /// <summary>
        /// The ID of the <see cref="Assignment"/> to which this <see cref="TimeRecord"/> belongs.
        /// </summary>
        public long? AssignmentId { get; set; }

        /// <summary>
        /// The ID of the <see cref="ClientUser"/> responsible for this employee time record.
        /// </summary>
        public long? ClientUserId { get; set; }

        /// <summary>
        /// The <see cref="ApplicationUser"/><c>.</c><see cref="ApplicationUser.Id"/> of who exported this TimeRecord in a report.
        /// </summary>
        public long? ExportedBy { get; set; }

        /// <summary>
        /// Wage type declaration.
        /// </summary>
        public string WageType { get; set; }

        /// <summary>
        /// Cost center.
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Amount of time or CHF associated with this <see cref="TimeRecord"/>.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Fixed wage amount.
        /// </summary>
        public decimal? AmountWageFixed { get; set; }

        /// <summary>
        /// Fixed tariff amount.
        /// </summary>
        public decimal? AmountTariffFixed { get; set; }

        /// <summary>
        /// Client-related amount.
        /// </summary>
        public decimal? AmountClient { get; set; }

        /// <summary>
        /// The type of shift associated with this <see cref="TimeRecord"/> (e.g. night shift, etc...).
        /// </summary>
        public long? ShiftId { get; set; }

        /// <summary>
        /// Is this entry to be recorded as the weekly maximum work time?
        /// </summary>
        public bool MaxHourEntry { get; set; }

        /// <summary>
        /// TBD!
        /// </summary>
        public long? CandidateId { get; set; }

        /// <summary>
        /// Id of the associated report document.
        /// </summary>
        public long? ReportDocumentId { get; set; }
    }
}