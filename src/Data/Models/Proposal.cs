using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using Newtonsoft.Json;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// An application dossier.
    /// </summary>
    [Table("Proposal")]
    public class Proposal : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a dossier.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The history of <see cref="Proposal"/> status changes.
        /// </summary>
        [Column("States", Order = 1)]
        public ICollection<ProposalStateHistory> States { get; set; }

        /// <summary>
        /// Gets the current (most recent) <see cref="ProposalStateHistory"/> attributed to this <see cref="Proposal"/>.
        /// </summary>
        [NotMapped]
        public ProposalStateHistory CurrentState => States?.OrderByDescending(state => state.Creation?.CreatedOn).FirstOrDefault();

        /// <summary>
        /// ID of the <see cref="SupplierUser"/> in charge of this <see cref="Proposal"/>.
        /// </summary>
        [Column("SupplierUserId", Order = 2)]
        public long? SupplierUserId { get; set; }

        /// <summary>
        /// The <see cref="SupplierUser"/> in charge of this <see cref="Proposal"/>.
        /// </summary>
        [ForeignKey("SupplierUserId")]
        public SupplierUser SupplierUser { get; set; }

        /// <summary>
        /// The <see cref="Employee"/> associated with this <see cref="Proposal"/>.
        /// </summary>
        [Column("EmployeeId", Order = 3)]
        public long? EmployeeId { get; set; }

        /// <summary>
        /// ID of the <see cref="Employee"/> associated with this <see cref="Proposal"/>.
        /// </summary>
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        /// <summary>
        /// Comment message string attached to this <see cref="Proposal"/>.
        /// </summary>
        [Column("Comment", Order = 4)]
        public string Comment { get; set; }

        /// <summary>
        /// The associated <see cref="PoolEmployeeDocument"/>s.
        /// </summary>
        [Column("PoolEmployeeDocuments", Order = 5)]
        public ICollection<PoolEmployeeDocument> PoolEmployeeDocuments { get; set; }

        /// <summary>
        /// <see cref="Proposal"/> attachments.
        /// </summary>
        [Column("ProposalDocuments", Order = 6)]
        public ICollection<ProposalDocument> ProposalDocuments { get; set; }

        /// <summary>
        /// The ID of the <see cref="Vacancy"/> to which this <see cref="Proposal"/> belongs.
        /// </summary>
        [Column("VacancyId", Order = 7)]
        public long? VacancyId { get; set; }

        /// <summary>
        /// The <see cref="Vacancy"/> to which this <see cref="Proposal"/> belongs.
        /// </summary>
        [ForeignKey("VacancyId"), JsonIgnore]
        public Vacancy Vacancy { get; set; }

        /// <summary>
        /// Related <see cref="Assignment"/> ID.
        /// </summary>
        [Column("AssignmentId", Order = 8)]
        public long? AssignmentId { get; set; }

        /// <summary>
        /// Related <see cref="Assignment"/>.
        /// </summary>
        [ForeignKey("AssignmentId")]
        public Assignment Assignment { get; set; }
    }
}