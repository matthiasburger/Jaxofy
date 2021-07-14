using DasTeamRevolution.Data.Models;

namespace DasTeamRevolution.Models.Dto.PoolEmployeeDocument
{
    /// <summary>
    /// Request DTO for fetching a list of all <see cref="PoolEmployeeDocument"/>s associated with a <see cref="PoolEmployee"/>.
    /// </summary>
    public class ListPoolEmployeeDocumentsRequestDto
    {
        /// <summary>
        /// <see cref="PoolEmployee"/>'s <see cref="PoolEmployee.Id"/>.
        /// </summary>
        public long PoolEmployeeId { get; set; }
    }
}