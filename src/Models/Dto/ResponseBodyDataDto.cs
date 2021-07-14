using System.Collections.Generic;

namespace Jaxofy.Models.Dto
{
    /// <summary>
    /// Data payload to return in an API response body.
    /// </summary>
    public class ResponseBodyDataDto
    {
        /// <summary>
        /// The type of the items returned.
        /// Can be <c>null</c> if the request was successful but nothing's returned (e.g. a status <c>201</c>).
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// The total amount of items potentially available to fetch.
        /// </summary>
        public long? Count { get; set; }
        
        /// <summary>
        /// This and <see cref="ResponseBodyDto.Errors"/> are mutually exclusive: if this is set, <see cref="ResponseBodyDto.Errors"/> should be <c>null</c> and vice-versa!<para> </para>
        /// This can be <c>null</c> if the request succeeded and there are no items to return.
        /// </summary>
        public IEnumerable<object> Items { get; set; }
    }
}