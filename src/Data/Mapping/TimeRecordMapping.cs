using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.TimeRecord;

namespace DasTeamRevolution.Data.Mapping
{
    public class TimeRecordMapping : Profile
    {
        public TimeRecordMapping()
        {
            CreateMap<TimeRecord, TimeRecordResponseDto>();
            CreateMap<TimeRecordRequestDto, TimeRecord>();
        }
    }
}