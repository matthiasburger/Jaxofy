using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Employee time table entry.
    /// </summary>
    [Table("TimeRecord")]
    public class TimeRecord : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a time table entry.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The history of <see cref="TimeRecordStateHistory"/> changes.
        /// </summary>
        [Column("States", Order = 1)]
        public ICollection<TimeRecordStateHistory> States { get; set; } = Array.Empty<TimeRecordStateHistory>();

        /// <summary>
        /// Gets the current (most recent) <see cref="TimeRecordStateHistory"/> attributed to this <see cref="TimeRecord"/>.
        /// </summary>
        public TimeRecordStateHistory GetCurrentState() => States.OrderByDescending(state => state.Creation.CreatedOn).FirstOrDefault();

        /// <summary>
        /// Time record creation metadata.
        /// </summary>
        [Column("Creation", Order = 2)]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last time record's modification metadata.
        /// </summary>
        [Column("LastModification", Order = 3)]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The ID of the <see cref="Assignment"/> to which this <see cref="TimeRecord"/> belongs.
        /// </summary>
        [Column("AssignmentId", Order = 4)]
        public long? AssignmentId { get; set; }

        /// <summary>
        /// The <see cref="Assignment"/> to which this <see cref="TimeRecord"/> belongs.
        /// </summary>
        [ForeignKey("AssignmentId")]
        public Assignment Assignment { get; set; }

        /// <summary>
        /// The ID of the <see cref="ClientUser"/> responsible for this employee time record.
        /// </summary>
        [Column("ClientUserId", Order = 5)]
        public long? ClientUserId { get; set; }

        /// <summary>
        /// The <see cref="ClientUser"/> responsible for this employee time record.
        /// </summary>
        [ForeignKey("ClientUserId")]
        public ClientUser ClientUser { get; set; }

        /// <summary>
        /// The <see cref="ApplicationUser"/><c>.</c><see cref="ApplicationUser.Id"/> of who exported this TimeRecord in a report.
        /// </summary>
        [Column("ExportedBy", Order = 6)]
        public long? ExportedBy { get; set; }

        /// <summary>
        /// Timestamp of when this <see cref="TimeRecord"/> was exported in a report (by <see cref="ApplicationUser"/> Id = s<see cref="ExportedBy"/>).
        /// </summary>
        [Column("ExportedOn", Order = 7)]
        public DateTime? ExportedOn { get; set; }

        /// <summary>
        /// Timestamp of the deletion of this <see cref="TimeRecord"/> db row. This is <c>null</c> if the entry was not deleted yet.
        /// </summary>
        [Column("RemovedOn", Order = 8)]
        public DateTime? RemovedOn { get; set; }

        /// <summary>
        /// Wage type declaration.
        /// </summary>
        [StringLength(50)]
        [Column("WageType", Order = 9)]
        public string WageType { get; set; }

        /// <summary>
        /// Cost center.
        /// </summary>
        [StringLength(300)]
        [Column("CostCenter", Order = 10)]
        public string CostCenter { get; set; }

        /// <summary>
        /// Amount of time or CHF associated with this <see cref="TimeRecord"/>.
        /// </summary>
        [Column("Amount", Order = 11, TypeName = "decimal(5,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Fixed wage amount.
        /// </summary>
        [Column("AmountWageFixed", Order = 12, TypeName = "decimal(5,2)")]
        public decimal? AmountWageFixed { get; set; }

        /// <summary>
        /// Fixed tariff amount.
        /// </summary>
        [Column("AmountTariffFixed", Order = 13, TypeName = "decimal(5,2)")]
        public decimal? AmountTariffFixed { get; set; }

        /// <summary>
        /// Client-related amount.
        /// </summary>
        [Column("AmountClient", Order = 14, TypeName = "decimal(5,2)")]
        public decimal? AmountClient { get; set; }

        /// <summary>
        /// The type of shift associated with this <see cref="TimeRecord"/> (e.g. night shift, etc...).
        /// </summary>
        [Column("ShiftId", Order = 15)]
        public long? ShiftId { get; set; }

        /// <summary>
        /// Is this entry to be recorded as the weekly maximum work time?
        /// </summary>
        [Column("MaxHourEntry", Order = 16)]
        public bool MaxHourEntry { get; set; }

        /// <summary>
        /// TBD!
        /// </summary>
        [Column("CandidateId", Order = 17)]
        public long? CandidateId { get; set; }

        /// <summary>
        /// Id of the associated report document.
        /// </summary>
        [Column("ReportDocumentId", Order = 18)]
        public long? ReportDocumentId { get; set; }
    }
}