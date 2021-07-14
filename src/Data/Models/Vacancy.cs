using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DasTeamRevolution.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A vacancy.
    /// </summary>
    [Table("Vacancy")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Vacancy : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a vacancy.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The history of <see cref="VacancyStateHistory"/> changes.
        /// </summary>
        [Column("States", Order = 1)]
        public ICollection<VacancyStateHistory> States { get; set; } = new List<VacancyStateHistory>();

        /// <summary>
        /// Gets the current (most recent) <see cref="VacancyStateHistory"/> attributed to this <see cref="Vacancy"/>.
        /// </summary>
        public VacancyStateHistory GetCurrentState() => States.OrderByDescending(state => state.Creation.CreatedOn).FirstOrDefault();

        /// <summary>
        /// Vacancy creation metadata.
        /// </summary>
        [Column("CreationId", Order = 2)]
        public long? CreationId { get; set; }
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last vacancy's modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 3)]
        public long? LastModificationId { get; set; }
        
        /// <summary>
        /// Last row record modification's metadata.
        /// </summary>
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// Uniquely indexed slug string that is human-readable and allows easy identification of a <see cref="Vacancy"/> inside the db.
        /// Can be used for e.g. more readable, more memorable URLs. 
        /// </summary>
        [Column("Slug", Order = 4)]
        public string Slug { get; set; }

        /// <summary>
        /// The <see cref="Client"/> to which this <see cref="Vacancy"/> is assigned.
        /// </summary>
        [Column("ClientId", Order = 5)]
        public long? ClientId { get; set; }
        
        /// <summary>
        /// The <see cref="Client"/> to which this <see cref="Vacancy"/> is assigned.
        /// </summary>
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        /// <summary>
        /// The vacancy's <see cref="PostalAddress"/>.
        /// </summary>
        [Column("PostalAddressId", Order = 6)]
        public long? PostalAddressId { get; set; }
        
        /// <summary>
        /// The vacancy's <see cref="PostalAddress"/>
        /// </summary>
        [ForeignKey("PostalAddressId")]
        public PostalAddress PostalAddress { get; set; }
        
        /// <summary>
        /// The dossiers assigned to this <see cref="Vacancy"/>.
        /// </summary>
        [Column("Proposals", Order = 7)]
        public ICollection<Proposal> Proposals { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the vacancy is active
        /// </summary>
        [Column("IsActive", Order = 8)]
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies deadline
        /// </summary>
        [Column("Deadline", Order = 9, TypeName = DatabaseType.DateTime2NoPrecision)]
        public DateTime? Deadline { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies starting date. Value has no time precision.
        /// </summary>
        [Column("StartDate", Order = 10, TypeName = DatabaseType.Date)]
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancies ending date. Value has no time precision.
        /// </summary>
        [Column("EndDate", Order = 11, TypeName = DatabaseType.Date)]
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Gets or sets the vacancy-title
        /// </summary>
        [Column("Title", Order = 12)]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets a description for the vacancy
        /// </summary>
        [Column("Description", Order = 13)]
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the amount of employees the vacancy requires
        /// </summary>
        [Column("RequiredAmountEmployees", Order = 14)]
        public int? RequiredAmountEmployees { get; set; }
        
        /// <summary>
        /// Gets or sets the job-profile-id
        /// </summary>
        [Column("JobProfileId", Order = 15)]
        public long? JobProfileId { get; set; }
        
        /// <summary>
        /// Gets or sets the job-profile
        /// </summary>
        public JobProfile JobProfile { get; set; }
    }
}