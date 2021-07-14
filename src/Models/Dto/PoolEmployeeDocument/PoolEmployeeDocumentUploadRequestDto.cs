using Jaxofy.Models.Dto.Document;

namespace Jaxofy.Models.Dto.PoolEmployeeDocument
{
    /// <summary>
    /// Request DTO for document uploading.
    /// </summary>
    public class PoolEmployeeDocumentUploadRequestDto
    {
        /// <summary>
        /// The document's content in bytes.
        /// </summary>
        public DocumentUploadDto Document { get; set; }
        
        /// <summary>
        /// The id of the <see cref="PoolEmployee"/> who shall be associated with this document. 
        /// </summary>
        public long PoolEmployeeId { get; set; }
    }
}