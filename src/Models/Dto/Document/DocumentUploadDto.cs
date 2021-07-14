namespace Jaxofy.Models.Dto.Document
{
    public class DocumentUploadDto
    {
        /// <summary>
        /// Gets or sets the filecontent
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets the contenttype 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the filename
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// Returns the length of content
        /// </summary>
        public int Length => Content?.Length ?? 0;
    }
}