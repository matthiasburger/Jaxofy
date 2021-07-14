using AutoMapper;
using Jaxofy.Data.Models;
using Jaxofy.Models.Dto.ApplicationUser;

namespace Jaxofy.Data.Mapping
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserResponseDto>();
        }   
    }
}