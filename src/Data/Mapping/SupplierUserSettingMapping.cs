using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierUserSettings;

namespace DasTeamRevolution.Data.Mapping
{
    public class SupplierUserSettingMapping : Profile
    {
        public SupplierUserSettingMapping()
        {
            CreateMap<SupplierUserSetting, SupplierUserSettingsResponseDto>();
            CreateMap<SupplierUserSettingsRequestDto, SupplierUserSetting>();
        }
    }
}