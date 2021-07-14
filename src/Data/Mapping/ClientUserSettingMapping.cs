using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientUserSetting;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientUserSettingMapping : Profile
    {
        public ClientUserSettingMapping()
        {
            CreateMap<ClientUserSetting, ClientUserSettingResponseDto>();
            CreateMap<ClientUserSettingRequestDto, ClientUserSetting>();
        }
    }
}