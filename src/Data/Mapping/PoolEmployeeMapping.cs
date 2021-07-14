using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.PoolEmployee;

namespace DasTeamRevolution.Data.Mapping
{
    public class PoolEmployeeMapping : Profile
    {
        public PoolEmployeeMapping()
        {
            CreateMap<PoolEmployee, PoolEmployeeResponseDto>();
            CreateMap<PoolEmployeeRequestDto, PoolEmployee>();
        }
    }
}