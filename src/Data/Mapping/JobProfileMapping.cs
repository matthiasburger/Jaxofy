using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.JobProfile;

namespace DasTeamRevolution.Data.Mapping
{
    public class JobProfileMapping : Profile
    {
        public JobProfileMapping()
        {
            CreateMap<JobProfile, JobProfileResponseDto>();
            CreateMap<JobProfileRequestDto, JobProfile>();
        }
    }
}