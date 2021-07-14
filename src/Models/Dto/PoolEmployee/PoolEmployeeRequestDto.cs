using System;
using Jaxofy.Models.Enums;

namespace Jaxofy.Models.Dto.PoolEmployee
{
    public class PoolEmployeeRequestDto
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
        /// Gets or sets the employees gender
        /// </summary>
        public Gender Gender { get; set; }
        
        
        /// <summary>
        /// Gets or sets a value indicating whether the employee is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}