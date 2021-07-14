using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DasTeamRevolution.Controllers.Base;
using DasTeamRevolution.Data;
using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Extensions;
using DasTeamRevolution.Models.Dto.ClientGroup;
using DasTeamRevolution.Services.AuthTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DasTeamRevolution.Controllers
{
    [ODataRouting]
    [Authorize]
    public class ClientGroupsController : ODataBaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _authTokenService;

        public ClientGroupsController(ApplicationDbContext db, IMapper mapper, IAuthTokenService authTokenService)
        {
            _db = db;
            _mapper = mapper;
            _authTokenService = authTokenService;
        }

        [HttpGet]
        public ActionResult<ClientGroupResponseDto> Get(ODataQueryOptions<ClientGroup> query, string[] join = null)
        {
            IQueryable<ClientGroup> clientGroupsQuery = _db.ClientGroups
                .Include(v => v.Creation)
                .Include(v => v.LastModification);

            if (join != null)
                clientGroupsQuery = join.Aggregate(clientGroupsQuery,
                    (current, table) => current.Include(table.Capitalize()));

            IQueryable clientGroups = query.ApplyTo(clientGroupsQuery);

            IList<ClientGroupResponseDto> clientGroupsResult = _mapper.Map<List<ClientGroupResponseDto>>(clientGroups);

            return EnvelopeResult.Ok((IEnumerable<ClientGroupResponseDto>)clientGroupsResult);
        }

        [HttpGet]
        public async Task<IActionResult> Get(long key)
        {
            ClientGroup clientGroup = await _db.ClientGroups
                .Include(v => v.Creation)
                .Include(v => v.LastModification)
                .SingleOrDefaultAsync(x => x.Id == key);

            if (clientGroup is null)
                return Error(HttpStatusCode.NotFound, Constants.Errors.ResourceNotFound<ClientGroup>(key));

            ClientGroupResponseDto clientGroupResult = _mapper.Map<ClientGroupResponseDto>(clientGroup);

            return EnvelopeResult.Ok(clientGroupResult);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientGroupRequestDto clientGroupRequest)
        {
            ClientGroup clientGroup = _mapper.Map<ClientGroup>(clientGroupRequest);
            long? userId = _authTokenService.ExtractUserId(HttpContext);

            if (!userId.HasValue)
            {
                return Forbidden();
            }

            // TODO: check user permission here!

            clientGroup.Creation = new RecordCreation
            {
                CreatedById = userId.Value,
                CreatedOn = DateTime.Now
            };

            await _db.ClientGroups.AddAsync(clientGroup);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Created($"/api/v1/clientgroups/{clientGroup.Id}", clientGroup);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long key, [FromBody] ClientGroupRequestDto clientGroupRequest)
        {
            ClientGroup clientGroup = _mapper.Map<ClientGroup>(clientGroupRequest);
            clientGroup.Id = key;
            
            ClientGroup storedClientGroup = await _db.ClientGroups
                .AsNoTracking()
                .SingleOrDefaultAsync(x=>x.Id == key);

            clientGroup.CreationId = storedClientGroup.CreationId;
            clientGroup.LastModificationId = storedClientGroup.LastModificationId;
            
            long? userId = _authTokenService.ExtractUserId(HttpContext);
            if (!userId.HasValue)
            {
                return Forbidden();
            }

            // TODO: check user permission here!
            
            clientGroup.LastModification = new RecordModification
            {
                Id = storedClientGroup.LastModificationId ?? 0,
                ModifiedById = userId,
                ModifiedOn = DateTime.Now
            };

            _db.ClientGroups.Update(clientGroup);
            await _db.SaveChangesAsync();

            return EnvelopeResult.Updated(clientGroup);
        }
    }
}