using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// An employee work assignment/deployment.
    /// </summary>
    [Table("Assignment")]
    public class Assignment : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies an employee deployment.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="Employee"/> subject to this <see cref="Assignment"/>.
        /// </summary>
        [Column("EmployeeId", Order = 1)]
        public long? EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        
        

        /// <summary>
        /// The time records entered for this <see cref="Assignment"/>.
        /// </summary>
        [Column("TimeRecords", Order = 2)]
        public ICollection<TimeRecord> TimeRecords { get; set; }

        /// <summary>
        /// The <see cref="Rating"/> for this <see cref="Assignment"/>.
        /// </summary>
        [Column("Rating", Order = 3)]
        public Rating Rating { get; set; }

        /// <summary>
        /// The ID of the <see cref="Order"/> which contains this <see cref="Assignment"/>.
        /// </summary>
        [Column("OrderId", Order = 4)]
        public long? OrderId { get; set; }

        /// <summary>
        /// The <see cref="Order"/> which contains this <see cref="Assignment"/>.
        /// </summary>
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Column("StartDate", Order = 5, TypeName = DatabaseType.Date)] 
        public DateTime StartDate { get; set; }
        [Column("EndDate", Order = 6, TypeName = DatabaseType.Date)] 
        public DateTime? EndDate { get; set; }

        [Column("Location", Order = 7), StringLength(300)]
        public string Location { get; set; }
        [Column("Salary", Order = 8), StringLength(300)]
        public string Salary { get; set; }
        [Column("CostCenter", Order = 9), StringLength(300)]
        public string CostCenter { get; set; }
        [Column("Department", Order = 10), StringLength(300)]
        public string Department { get; set; }
        [Column("ContactName", Order = 11), StringLength(300)]
        public string ContactName { get; set; }
        [Column("AssignmentAs", Order = 12), StringLength(300)]
        public string AssignmentAs { get; set; }
        [Column("Notes", Order = 13)]
        public string Notes { get; set; }
        
        [Column("ProposalId", Order = 14)]
        public long? ProposalId { get; set; }
        
        // [ForeignKey("ProposalId")]
        public Proposal Proposal { get; set; }
    }
}