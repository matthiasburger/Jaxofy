using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.SupplierHeader;

namespace DasTeamRevolution.Data.Mapping
{
    public class SupplierHeaderMapping : Profile
    {
        public SupplierHeaderMapping()
        {
            CreateMap<SupplierHeader, SupplierHeaderResponseDto>();
            CreateMap<SupplierHeaderRequestDto, SupplierHeader>();
        }
    }
}