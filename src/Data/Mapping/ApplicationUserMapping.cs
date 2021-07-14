using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ApplicationUser;

namespace DasTeamRevolution.Data.Mapping
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserResponseDto>();
        }   
    }
}