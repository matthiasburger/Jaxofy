using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Proposal;

namespace DasTeamRevolution.Data.Mapping
{
    public class ProposalMapping : Profile
    {
        public ProposalMapping()
        {
            CreateMap<Proposal, ProposalResponseDto>()
                .ForMember(x => x.CurrentState, expression => expression.MapFrom(x => x.CurrentState))
                .ForMember(x => x.Supplier, expression => expression.MapFrom(x => x.Employee.PoolEmployee.Supplier))
                .ForMember(x => x.Employee, expression => expression.MapFrom(x => x.Employee));

            CreateMap<ProposalStateHistory, ProposalStateResponseDto>();

            CreateMap<ProposalStateHistory, ProposalStateResponseDto>();
        }
    }
}