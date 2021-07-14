using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierUser;

namespace DasTeamRevolution.Data.Mapping
{
    public class SupplierUserMapping : Profile
    {
        public SupplierUserMapping()
        {
            CreateMap<SupplierUser, SupplierUserResponseDto>();
            CreateMap<SupplierUserRequestDto, SupplierUser>();
        }
    }
}