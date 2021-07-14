namespace DasTeamRevolution.Models.Dto.EmployeeDocument
{
    /// <summary>
    /// Response body DTO for successful document uploads.
    /// </summary>
    public class EmployeeDocumentUploadResponseDto
    {
        /// <summary>
        /// The ID of the document that was successfully uploaded.
        /// </summary>
        public long DocumentId { get; set; }
    }
}