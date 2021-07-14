using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.Employee;

namespace DasTeamRevolution.Data.Mapping
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            CreateMap<Employee, SlimmedEmployeeResponseDto>()
                .ForMember(x => x.PostalAddress, expression => expression.MapFrom(x => x.PostalAddress));
            
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(c => c.Supplier, expression => expression.MapFrom(x => x.PoolEmployee.Supplier));
            
            CreateMap<EmployeeRequestDto, Employee>();
        }
    }
}