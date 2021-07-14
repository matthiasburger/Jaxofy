using Jaxofy.Models.Dto.PostalAddress;

namespace Jaxofy.Models.Dto.Employee
{
    public class SlimmedEmployeeResponseDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PostalAddressResponseDto PostalAddress { get; set; }
    }
}