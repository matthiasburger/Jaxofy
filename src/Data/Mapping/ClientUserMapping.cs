using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientUser;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientUserMapping : Profile
    {
        public ClientUserMapping()
        {
            CreateMap<ClientUser, ClientUserResponseDto>();
            CreateMap<ClientUserRequestDto, ClientUser>();
        }
    }
}