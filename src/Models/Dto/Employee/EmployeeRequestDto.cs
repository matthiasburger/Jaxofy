using System;
using System.ComponentModel.DataAnnotations;
using DasTeamRevolution.Controllers;
using DasTeamRevolution.Models.Dto.PostalAddress;
using DasTeamRevolution.Models.Enums;

namespace DasTeamRevolution.Models.Dto.Employee
{
    /// <summary>
    /// DTO for the POST employee request endpoint inside <see cref="EmployeesController"/>.
    /// </summary>
    public class EmployeeRequestDto
    {
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
        /// The employee's gender (Dt.: "Anrede").
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The employee's address.
        /// </summary>
        public PostalAddressRequestDto PostalAddress { get; set; }

        /// <summary>
        /// The employee's AHV-Nr.
        /// </summary>
        [MaxLength(16)]
        public string AhvNumber { get; set; }
    }
}