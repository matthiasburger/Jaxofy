namespace Jaxofy.Models.Dto.PostalAddress
{
    public class PostalAddressRequestDto
    {
        /// <summary>
        /// Address (Name field) 
        /// </summary>
        public string PostalName { get; set; }

        /// <summary>
        /// Address (Street + house nr. field) 
        /// </summary>
        public string PostalStreet { get; set; }

        /// <summary>
        /// Address (ZIP code field) 
        /// </summary>
        public string PostalZipCode { get; set; }

        /// <summary>
        /// Address (City name field) 
        /// </summary>
        public string PostalCity { get; set; }

        /// <summary>
        /// Country-code as per ISO 3166 (either expressed as Alpha-2 code or Alpha-3).
        /// </summary>
        public string CountryCodeISO { get; set; }
        
        /// <summary>
        /// Converts the postal address fields into a normalized string to use for comparing.
        /// </summary>
        /// <returns>Normalized address string.</returns>
        public string GetNormalizedAddressString()
        {
            return $"{CountryCodeISO.ToUpperInvariant()}|{PostalZipCode}|{PostalCity}|{PostalStreet}|{PostalName}";
        }
    }
}