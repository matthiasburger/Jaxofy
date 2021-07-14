using AutoMapper;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.TimeRecordStateHistory;

namespace DasTeamRevolution.Data.Mapping
{
    public class TimeRecordStateHistoryMapping : Profile
    {
        public TimeRecordStateHistoryMapping()
        {
            CreateMap<TimeRecordStateHistory, TimeRecordStateHistoryResponseDto>();
        }
    }
}