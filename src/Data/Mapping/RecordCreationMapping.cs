using AutoMapper;

using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.RecordCreation;

namespace DasTeamRevolution.Data.Mapping
{
    public class RecordCreationMapping : Profile
    {
        public RecordCreationMapping()
        {
            CreateMap<RecordCreation, RecordCreationResponseDto>();
        }
    }
}