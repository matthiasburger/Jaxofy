using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Supplier;

namespace DasTeamRevolution.Data.Mapping
{
    public class SupplierMapping : Profile
    {
        public SupplierMapping()
        {
            CreateMap<Supplier, SupplierResponseDto>();
            CreateMap<Supplier, SlimmedSupplierResponseDto>()
                .ForMember(c=>c.Name, expression => expression.MapFrom(c=>c.Header.Name + " " +(c.PostalAddress == null ? "" : c.PostalAddress.PostalName ?? c.PostalAddress.PostalCity)));
            CreateMap<SupplierRequestDto, Supplier>();
        }
    }
}