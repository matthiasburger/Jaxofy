using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientSupplier;

namespace DasTeamRevolution.Data.Mapping
{
    public class ClientSupplierMapping : Profile
    {
        public ClientSupplierMapping()
        {
            CreateMap<ClientSupplier, ClientSupplierResponseDto>();
            CreateMap<ClientSupplierRequestDto, ClientSupplier>();
        }
    }
}