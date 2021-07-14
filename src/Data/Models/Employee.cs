using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Employee, often referred to as "TMA" (temporary employee).
    /// </summary>
    [Table("Employee")]
    [Index(nameof(LastName), IsUnique = false)]
    [Index(nameof(FirstName), IsUnique = false)]
    public class Employee : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies an employee (aka "TMA").
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The <see cref="Proposal"/>s associated with this <see cref="Employee"/>.
        /// </summary>
        [Column("Proposals", Order = 1)]
        public ICollection<Proposal> Proposals { get; set; }

        /// <summary>
        /// The <see cref="Assignment"/>s associated with this <see cref="Employee"/>.
        /// </summary>
        [Column("Assignments", Order = 2)]
        public ICollection<Assignment> Assignments { get; set; }

        /// <summary>
        /// Gets or sets the employee's lastname
        /// </summary>
        [Column("LastName", Order = 3)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the employee's firstname
        /// </summary>
        [Column("FirstName", Order = 4)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the employee's email-address
        /// </summary>
        [Column("Email", Order = 5)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the employee's phone-number
        /// </summary>
        [Column("Phone", Order = 6)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the employee's mobile-number
        /// </summary>
        [Column("Mobile", Order = 7)]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the employee's date of birth
        /// </summary>
        [Column("DateOfBirth", Order = 8)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 9)]
        public long? CreationId { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 10)]
        public long? LastModificationId { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// The source <see cref="PoolEmployee"/>'s ID.
        /// </summary>
        [Column("PoolEmployeeId", Order = 11)]
        public long? PoolEmployeeId { get; set; }

        /// <summary>
        /// The source <see cref="PoolEmployee"/>.
        /// </summary>
        [ForeignKey("PoolEmployeeId")]
        public PoolEmployee PoolEmployee { get; set; }

        /// <summary>
        /// The ID of the <see cref="Client"/> with which this <see cref="Employee"/> is associated.
        /// </summary>
        [Column("ClientId", Order = 12)]
        public long? ClientId { get; set; }

        /// <summary>
        /// The <see cref="Client"/> to which this <see cref="Employee"/> belongs.
        /// </summary>
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        /// <summary>
        /// Related supplier ID.
        /// </summary>
        [NotMapped]
        public long? SupplierId => PoolEmployee?.SupplierId;
        
        public string FullName => FirstName + " " + LastName;

        public bool HasAssignments => Assignments?.Any() ?? false;

        /// <summary>
        /// The employee's gender (Dt.: "Anrede").
        /// </summary>
        [Column("Gender", Order = 13)]
        public Gender Gender { get; set; }

        /// <summary>
        /// Foreign key of the <see cref="PostalAddress"/> associated with this <see cref="Employee"/>.
        /// </summary>
        [Column("PostalAddressId", Order = 14)]
        public long? PostalAddressId { get; set; }

        /// <summary>
        /// The employee's address.
        /// </summary>
        [ForeignKey("PostalAddressId")]
        public PostalAddress PostalAddress { get; set; }

        /// <summary>
        /// The employee's AHV-Nr.
        /// </summary>
        [Column("AhvNumber", Order = 15)]
        [MaxLength(16)]
        public string AhvNumber { get; set; }
    }
}