using System;

namespace Jaxofy.Models.Dto.ApplicationUser
{
    public class ApplicationUserResponseDto
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the users email-address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date, when the user was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the users Id who created the user.
        /// </summary>
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an administrator or not.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value, when the users has to change his password.
        /// If the value is <c>null</c>, the user does not have to change his password at all.
        /// </summary>
        public DateTime? NewPasswordRequiredOn { get; set; }
    }
}