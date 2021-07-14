using DasTeamRevolution.Models.Dto.PostalAddress;
using DasTeamRevolution.Models.Dto.RecordCreation;
using DasTeamRevolution.Models.Dto.RecordModification;

namespace DasTeamRevolution.Models.Dto.Client
{
    public class ClientResponseDto
    {
        public long Id { get; set; }

        public long? GroupId { get; set; }
    
        public RecordCreationResponseDto Creation { get; set; }

        public RecordModificationResponseDto LastModification { get; set; }

        public PostalAddressResponseDto PostalAddress { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

    }
}