namespace Jaxofy.Models.Dto.PoolEmployeeDocument
{
    /// <summary>
    /// Response body DTO for successful document uploads.
    /// </summary>
    public class PoolEmployeeDocumentUploadResponseDto
    {
        /// <summary>
        /// The ID of the document that was successfully uploaded.
        /// </summary>
        public long DocumentId { get; set; }
    }
}