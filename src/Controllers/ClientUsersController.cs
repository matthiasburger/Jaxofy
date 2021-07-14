using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ClientUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class ClientUsersController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ClientUsersController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IQueryable<IActionResult>> Get(ODataQueryOptions<ClientUser> query)
        {
            IQueryable clientUsers = query.ApplyTo(
                _db.ClientUsers.AsQueryable()
            );

            return EnvelopeResult.Ok((IEnumerable<ClientUserResponseDto>)_mapper.Map<List<ClientUserResponseDto>>(clientUsers));
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            ClientUser clientUser = await _db.ClientUsers.FirstOrDefaultAsync(x => x.Id == key);
            return EnvelopeResult.Ok(_mapper.Map<ClientUserResponseDto>(clientUser));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientUserRequestDto clientUserRequest)
        {
            ClientUser clientUser = _mapper.Map<ClientUser>(clientUserRequest);

            await _db.ClientUsers.AddAsync(clientUser);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clientusers/{clientUser.Id}", clientUser);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] ClientUserRequestDto clientUserRequest)
        {
            ClientUser clientUser = _mapper.Map<ClientUser>(clientUserRequest);
            clientUser.Id = key;

            _db.ClientUsers.Update(clientUser);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(clientUser);
        }
    }
}