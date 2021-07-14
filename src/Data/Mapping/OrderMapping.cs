using System.Linq;
using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Order;

namespace DasTeamRevolution.Data.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order, OrderResponseDto>()
                .ForMember(x => x.AssignmentsCount,
                    x => x.MapFrom(src => src.Assignments.LongCount()))
                .ForMember(x => x.AssignmentIds,
                    x => x.MapFrom(src => src.Assignments.Select(y => y.Id).ToList()));
            CreateMap<OrderRequestDto, Order>();
        }
    }
}