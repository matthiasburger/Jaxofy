using DasTeamRevolution.Models.Dto.Document;

namespace DasTeamRevolution.Models.Dto.EmployeeDocument
{
    /// <summary>
    /// Request DTO for document uploading.
    /// </summary>
    public class EmployeeDocumentUploadRequestDto
    {
        /// <summary>
        /// The document's content in bytes.
        /// </summary>
        public DocumentUploadDto Document { get; set; }
        
        /// <summary>
        /// The id of the <see cref="Employee"/> who shall be associated with this document. 
        /// </summary>
        public long EmployeeId { get; set; }
    }
}