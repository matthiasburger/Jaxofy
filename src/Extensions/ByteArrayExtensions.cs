using System;

namespace Jaxofy.Extensions
{
    /// <summary>
    /// Byte array extension methods.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a <c>byte[]</c> array to a Base64-encoded <see cref="string"/> with optional removal of the padding '=' characters.
        /// </summary>
        /// <param name="bytes">The bytes to encode.</param>
        /// <param name="omitPaddingChars">Should the <c>=</c> padding characters be omitted?</param>
        /// <returns>The Base64-encoded string.</returns>
        public static string ToBase64String(this byte[] bytes, bool omitPaddingChars = false)
        {
            string output = Convert.ToBase64String(bytes);
            return omitPaddingChars ? output.TrimEnd('=') : output;
        }
    }
}
