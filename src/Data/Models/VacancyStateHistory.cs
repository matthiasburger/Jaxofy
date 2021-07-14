using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Enums;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// <see cref="Vacancy"/> state record entry.
    /// </summary>
    [Table("VacancyStateHistory")]
    public class VacancyStateHistory : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a vacancy's state.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="VacancyStateType"/>.
        /// </summary>
        [Column("StateType", Order = 1)]
        public VacancyStateType StateType { get; set; }
        
        /// <summary>
        /// The vacancy state modification entry's author and timestamp.
        /// </summary>
        [Column("Creation", Order = 2)]
        public RecordCreation Creation { get; set; }
        
        /// <summary>
        /// The <see cref="Vacancy"/> to which this state record belongs.
        /// </summary>
        [Column("Vacancy", Order = 3)]
        public Vacancy Vacancy { get; set; }
    }
}