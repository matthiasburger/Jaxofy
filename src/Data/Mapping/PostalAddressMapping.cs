using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PostalAddress;

namespace DasTeamRevolution.Data.Mapping
{
    public class PostalAddressMapping : Profile
    {
        public PostalAddressMapping()
        {
            CreateMap<PostalAddress, PostalAddressResponseDto>();
            CreateMap<PostalAddressRequestDto, PostalAddress>();
        }
    }
}