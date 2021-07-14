using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientGroup;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientGroupMapping : Profile
    {
        public ClientGroupMapping()
        {
            CreateMap<ClientGroup, ClientGroupResponseDto>();
            CreateMap<ClientGroupRequestDto, ClientGroup>();
        }
    }
}