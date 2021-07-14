namespace Jaxofy.Models.Dto.EmployeeDocument
{
    /// <summary>
    /// Request DTO for fetching a list of all <see cref="EmployeeDocument"/>s associated with an <see cref="Employee"/>.
    /// </summary>
    public class ListEmployeeDocumentsRequestDto
    {
        /// <summary>
        /// <see cref="Employee"/>'s <see cref="Employee.Id"/>.
        /// </summary>
        public long EmployeeId { get; set; }
    }
}