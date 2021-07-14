using DasTeamRevolution.Models.Dto.PostalAddress;

namespace DasTeamRevolution.Models.Dto.Client
{
    public class ClientRequestDto
    {
        public long? GroupId { get; set; }
        
        public PostalAddressRequestDto PostalAddress { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }
}