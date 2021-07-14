using System;
using System.Collections.Generic;
using System.Linq;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Models.Dto.General;
using DasTeamRevolution.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DasTeamRevolution.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VacancyStatesController : ApiBaseController
    {
        [HttpGet]
        public IActionResult GetVacancyStates()
        {
            IEnumerable<KeyValueSet<int, string>> vacancyStateDictionary = Enum.GetValues(typeof(VacancyStateType))
                .Cast<VacancyStateType>()
                .Select(
                    x => new KeyValueSet<int, string>
                    {
                        Id = (int) x,
                        Value = x.ToString()
                    }
                ).ToList();

            return EnvelopeResult.Ok(vacancyStateDictionary);
        }

        [HttpPut]
        public IActionResult UpdateVacancyState()
        {
            throw new NotImplementedException("changing vacancies state is not implemented yet");
        }
    }
}