using Jaxofy.Models.Dto.PostalAddress;
using Jaxofy.Models.Dto.RecordCreation;
using Jaxofy.Models.Dto.RecordModification;

namespace Jaxofy.Models.Dto.Client
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