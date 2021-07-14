using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientHeader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class ClientHeadersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ClientHeadersController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(ODataQueryOptions<ClientHeader> query)
        {
            IQueryable clientHeaders = query.ApplyTo(
                _db.ClientHeaders.AsQueryable()
            );

            IEnumerable<ClientHeaderResponseDto> clientHeadersResponse = 
                _mapper.Map<List<ClientHeaderResponseDto>>(clientHeaders);
            return EnvelopeResult.Ok(clientHeadersResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            ClientHeader clientHeader = await _db.ClientHeaders
                .SingleOrDefaultAsync(x => x.Id == key);

            if (clientHeader is null)
                return Error(HttpStatusCode.NotFound,Constants.Errors.ResourceNotFound<ClientHeader>(key));
            
            ClientHeaderResponseDto clientHeaderResponse = _mapper.Map<ClientHeaderResponseDto>(clientHeader);
            return EnvelopeResult.Ok(clientHeaderResponse);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientHeaderRequestDto clientHeaderRequest)
        {
            ClientHeader clientHeader = _mapper.Map<ClientHeader>(clientHeaderRequest);
            
            await _db.ClientHeaders.AddAsync(clientHeader);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clientheaders/{clientHeader.Id}", clientHeader);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] ClientHeaderRequestDto clientHeaderRequest)
        {
            ClientHeader clientHeader = _mapper.Map<ClientHeader>(clientHeaderRequest);
            clientHeader.Id = key;

            _db.ClientHeaders.Update(clientHeader);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(clientHeader);
        }
    }
}