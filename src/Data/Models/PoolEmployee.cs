using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Global pool employee related to a <see cref="SupplierHeader"/>.
    /// </summary>
    [Table("PoolEmployee")]
    [Index(nameof(LastName), IsUnique = false)]
    [Index(nameof(FirstName), IsUnique = false)]
    public class PoolEmployee : IEntity
    {
        /// <summary>
        /// Primary key that uniquely identifies a <see cref="PoolEmployee"/>.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The documents concerning this <see cref="PoolEmployee"/>.
        /// </summary>
        [Column("Documents", Order = 1)]
        public ICollection<PoolEmployeeDocument> Documents { get; set; }

        /// <summary>
        /// DB row creation metadata.
        /// </summary>
        [Column("CreationId", Order = 2)]
        public long? CreationId { get; set; }
        
        [ForeignKey("CreationId")]
        public RecordCreation Creation { get; set; }

        /// <summary>
        /// Last data entry modification metadata.
        /// </summary>
        [Column("LastModificationId", Order = 3)]
        public long? LastModificationId { get; set; }
        
        [ForeignKey("LastModificationId")]
        public RecordModification LastModification { get; set; }

        /// <summary>
        /// Gets or sets the employee's lastname
        /// </summary>
        [Column("LastName", Order = 4)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the employee's firstname
        /// </summary>
        [Column("FirstName", Order = 5)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the employee's email-address
        /// </summary>
        [Column("Email", Order = 6)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the employee's phone-number
        /// </summary>
        [Column("Phone", Order = 7)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the employee's mobile-number
        /// </summary>
        [Column("Mobile", Order = 8)]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the employee's date of birth
        /// </summary>
        [Column("DateOfBirth", Order = 9)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the Supplier-Id
        /// </summary>
        [Column("SupplierId", Order = 10)]
        public long SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
        
        /// <summary>
        /// Gets or sets the employees gender
        /// </summary>
        [Column("Gender", Order = 11)]
        public Gender Gender { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the employee is active
        /// </summary>
        [Column("IsActive", Order = 12)]
        public bool IsActive { get; set; }
    }
}