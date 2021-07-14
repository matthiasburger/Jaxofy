using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// A customer branch profile. 
    /// </summary>
    [Table("EmployeeProfile")]
    public class EmployeeProfile : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a profile.
        /// </summary>
        [Key, Column("Id", Order = 0)] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }  
    }
}