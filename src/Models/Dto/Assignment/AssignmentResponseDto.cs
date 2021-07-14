using System;
using Jaxofy.Models.Dto.Client;
using Jaxofy.Models.Dto.Employee;

namespace Jaxofy.Models.Dto.Assignment
{
    public class AssignmentResponseDto
    {
        public long Id { get; set; }
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
        
        public EmployeeResponseDto Employee { get; set; }
        public SlimmedClientResponseDto Client { get; set; }
    }
}