using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.RecordModification;

namespace DasTeamRevolution.Data.Mapping
{
    public class RecordModificationMapping : Profile
    {
        public RecordModificationMapping()
        {
            CreateMap<RecordModification, RecordModificationResponseDto>();
        }
    }
}