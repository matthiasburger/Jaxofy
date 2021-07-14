using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Vacancy;

namespace DasTeamRevolution.Data.Mapping
{
    public class VacancyMapping : Profile
    {
        public VacancyMapping()
        {
            CreateMap<Vacancy, VacancyResponseDto>()
                .ForMember(x=>x.CurrentState, expression => expression.MapFrom(x=>x.GetCurrentState()));
            CreateMap<VacancyRequestDto, Vacancy>();

            CreateMap<VacancyStateHistory, VacancyStateResponseDto>();
        }
    }
}