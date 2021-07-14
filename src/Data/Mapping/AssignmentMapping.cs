using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Assignment;

namespace DasTeamRevolution.Data.Mapping
{
    public class AssignmentMapping : Profile
    {
        public AssignmentMapping()
        {
            CreateMap<Assignment, AssignmentResponseDto>()
                .ForMember(x => x.Client,
                    expression => expression.MapFrom(x => x.Order == null ? null : x.Order.Client));
            CreateMap<AssignmentRequestDto, Assignment>();
        }
    }
}