using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Client;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientMapping : Profile
    {
        public ClientMapping()
        {
            CreateMap<Client, ClientResponseDto>();
            CreateMap<Client, SlimmedClientResponseDto>()
                .ForMember(c=>c.Name, x=>x.MapFrom(src=>
                    $"{src.Group.Name} {(src.PostalAddress != null ? (src.PostalAddress.PostalName ?? src.PostalAddress.PostalCity) : "")}"));
            CreateMap<ClientRequestDto, Client>();
        }
    }
}