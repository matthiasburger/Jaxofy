using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DasTeamRevolution.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Entity containing users from the application.
    /// </summary>
    [Table("ApplicationUser")]
    [Index(nameof(Email), IsUnique = true)]
    public class ApplicationUser : IEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the users email-address.
        /// </summary>
        [Column("Email", Order = 1)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Column("FirstName", Order = 2)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Column("LastName", Order = 3)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date, when the user was created.
        /// </summary>
        [Column("CreatedOn", Order = 4, TypeName = DatabaseType.DateTime2NoPrecision)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the users Id who created the user.
        /// </summary>
        [Column("CreatedBy", Order = 5)]
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not.
        /// </summary>
        [Column("IsActive", Order = 6)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the users password.
        /// </summary>
        [Column("Password", Order = 7)]
        [JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an administrator or not.
        /// </summary>
        [Column("IsAdmin", Order = 8)]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value, when the users has to change his password.
        /// If the value is <c>null</c>, the user does not have to change his password at all.
        /// </summary>
        [Column("NewPasswordRequiredOn", Order = 9, TypeName = DatabaseType.Date)]
        public DateTime? NewPasswordRequiredOn { get; set; }

        /// <summary>
        /// If this is <c>null</c>, the user is a <see cref="SupplierUser"/>.
        /// </summary>
        [Column("ClientUser", Order = 10)]
        public ClientUser ClientUser { get; set; }

        /// <summary>
        /// If this is <c>null</c>, the user is a <see cref="ClientUser"/>.
        /// </summary>
        [Column("SupplierUser", Order = 11)]
        public SupplierUser SupplierUser { get; set; }
    }
}