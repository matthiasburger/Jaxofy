using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasTeamRevolution.Data.Models.Base;
using DasTeamRevolution.Models.Dto.PostalAddress;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Data.Models
{
    /// <summary>
    /// Domain model containing a postal address.
    /// </summary>
    [Table("PostalAddress")]
    [Index(nameof(CountryCodeISO), IsUnique = false)]
    [Index(nameof(PostalZipCode), nameof(PostalCity), IsUnique = false)]
    public class PostalAddress : IEntity, IEquatable<PostalAddress>
    {
        /// <summary>
        /// Primary key that uniquely identifies a postal address.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Address (Name field) 
        /// </summary>
        [Column("PostalName", Order = 1)]
        [MaxLength(256)]
        public string PostalName { get; set; }

        /// <summary>
        /// Address (Street + house nr. field) 
        /// </summary>
        [Column("PostalStreet", Order = 2)]
        [MaxLength(256)]
        public string PostalStreet { get; set; }

        /// <summary>
        /// Address (ZIP code field) 
        /// </summary>
        [Column("PostalZipCode", Order = 3)]
        [MaxLength(16)]
        public string PostalZipCode { get; set; }

        /// <summary>
        /// Address (City name field) 
        /// </summary>
        [Column("PostalCity", Order = 4)]
        [MaxLength(64)]
        public string PostalCity { get; set; }

        /// <summary>
        /// Country-code as per ISO 3166 (either expressed as Alpha-2 code or Alpha-3).
        /// </summary>
        [MinLength(2)]
        [MaxLength(3)]
        [Column("CountryCodeISO", Order = 5)]
        public string CountryCodeISO { get; set; }

        /// <summary>
        /// <see cref="PostalAddress"/>es can be implicitly converted into their ID-less creation DTO counterform.
        /// </summary>
        /// <seealso cref="PostalAddressRequestDto"/>
        /// <param name="postalAddress">The address to convert.</param>
        /// <returns>The converted <see cref="PostalAddressRequestDto"/> instance.</returns>
        public static implicit operator PostalAddressRequestDto(PostalAddress postalAddress)
        {
            return new()
            {
                PostalName = postalAddress.PostalName,
                PostalStreet = postalAddress.PostalStreet,
                PostalZipCode = postalAddress.PostalZipCode,
                PostalCity = postalAddress.PostalCity,
                CountryCodeISO = postalAddress.CountryCodeISO
            };
        }

        public bool Equals(PostalAddress other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(PostalName, other.PostalName, StringComparison.OrdinalIgnoreCase) ///////////
                   && string.Equals(PostalStreet, other.PostalStreet, StringComparison.OrdinalIgnoreCase) ////
                   && string.Equals(PostalZipCode, other.PostalZipCode, StringComparison.OrdinalIgnoreCase) //
                   && string.Equals(PostalCity, other.PostalCity, StringComparison.OrdinalIgnoreCase) ////////
                   && string.Equals(CountryCodeISO, other.CountryCodeISO, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((PostalAddress) obj);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(CountryCodeISO, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(PostalZipCode, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(PostalCity, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(PostalStreet, StringComparer.OrdinalIgnoreCase);
            hashCode.Add(PostalName, StringComparer.OrdinalIgnoreCase);
            return hashCode.ToHashCode();
        }
    }
}