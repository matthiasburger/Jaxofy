using System;

namespace DasTeamRevolution.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns just the raw ASN.1 BER-encoded bytes from a PKCS#1/PKCS#8 key structure.
        /// </summary>
        /// <param name="pem">The PEM-formatted key string to convert.</param>
        /// <returns>ASN.1 (BER) encoded bytes.</returns>
        public static byte[] ToASN1bytesFromPEM(this string pem)
        {
            string base64 = pem
                .Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----END RSA PRIVATE KEY-----", string.Empty)
                .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                .Replace("-----END PRIVATE KEY-----", string.Empty)
                .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
                .Replace("-----END PUBLIC KEY-----", string.Empty)
                .Replace("\n", string.Empty);

            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Converts a base64 string into a byte array, padding it with trailing <c>=</c> signs at the end if necessary.
        /// </summary>
        /// <param name="b64">The base64 string to decode.</param>
        /// <returns>Decoded <c>byte[]</c></returns>
        public static byte[] ToBytesFromBase64(this string b64)
        {
            while (b64.Length % 4 != 0)
            {
                b64 += '=';
            }

            return Convert.FromBase64String(b64);
        }

        public static string Capitalize(this string @this)
        {
            return @this.Length switch
            {
                0 => null,
                1 => $"{char.ToUpper(@this[0])}",
                _ => $"{char.ToUpper(@this[0])}{@this[1..]}"
            };
        }
    }
}