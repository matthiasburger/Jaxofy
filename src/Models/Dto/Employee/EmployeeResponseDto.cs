using System;
using System.ComponentModel.DataAnnotations;
using DasTeamRevolution.Controllers;
using DasTeamRevolution.Models.Dto.PostalAddress;
using DasTeamRevolution.Models.Dto.Supplier;
using DasTeamRevolution.Models.Enums;

namespace DasTeamRevolution.Models.Dto.Employee
{
    /// <summary>
    /// DTO for the GET employee endpoint inside <see cref="EmployeesController"/>.
    /// </summary>
    public class EmployeeResponseDto
    {
        /// <summary>
        /// Primary key that uniquely identifies a <see cref="PoolEmployee"/>.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the employee's lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the employee's firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the employee's email-address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the employee's phone-number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the employee's mobile-number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the employee's date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the Supplier-Id
        /// </summary>
        public long SupplierId { get; set; }
        
        /// <summary>
        /// The ID of the related (originating) <see cref="PoolEmployee"/>.
        /// </summary>
        public long PoolEmployeeId { get; set; }
        
        /// <summary>
        /// The employee's gender (Dt.: "Anrede").
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// ID of the employee's postal address.
        /// </summary>
        public long? PostalAddressId { get; set; }
        
        /// <summary>
        /// The employee's address.
        /// </summary>
        public PostalAddressResponseDto PostalAddress { get; set; }

        /// <summary>
        /// The employee's AHV-Nr.
        /// </summary>
        [MaxLength(16)]
        public string AhvNumber { get; set; }
        
        
        public SlimmedSupplierResponseDto Supplier { get; set; }
    }
}