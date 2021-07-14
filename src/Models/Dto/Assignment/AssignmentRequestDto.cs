using System;

namespace DasTeamRevolution.Models.Dto.Assignment
{
    public class AssignmentRequestDto
    {
        public long? EmployeeId { get; set; }
        public long? OrderId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Salary { get; set; }
        public string CostCenter { get; set; }
        public string Department { get; set; }
        public string ContactName { get; set; }
        public string AssignmentAs { get; set; }
        public string Notes { get; set; }
    }
}