using DasTeamRevolution.Models.Dto.PostalAddress;

namespace DasTeamRevolution.Models.Dto.Employee
{
    public class SlimmedEmployeeResponseDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PostalAddressResponseDto PostalAddress { get; set; }
    }
}