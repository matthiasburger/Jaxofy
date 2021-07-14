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
    public class ProposalStatesController : ApiBaseController
    {
        [HttpGet]
        public IActionResult GetProposalStates()
        {
            IEnumerable<KeyValueSet<int, string>> proposalStateDictionary = Enum.GetValues(typeof(ProposalStateType))
                .Cast<ProposalStateType>()
                .Select(
                    x => new KeyValueSet<int, string>
                    {
                        Id = (int) x,
                        Value = x.ToString()
                    }
                ).ToList();

            return EnvelopeResult.Ok(proposalStateDictionary);
        }

        [HttpPut]
        public IActionResult UpdateProposalState()
        {
            throw new NotImplementedException("changing proposal state is not implemented yet");
        }
    }
}