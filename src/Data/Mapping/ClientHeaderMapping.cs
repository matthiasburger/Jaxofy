using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientHeader;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientHeaderMapping : Profile
    {
        public ClientHeaderMapping()
        {
            CreateMap<ClientHeader, ClientHeaderResponseDto>();
            CreateMap<ClientHeaderRequestDto, ClientHeader>();
        }
    }
}